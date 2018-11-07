using UnityEngine;

public class HeatImaging : MonoBehaviour
{
    private Camera cam;
    private Camera outlineBufferCam;

    [SerializeField]
    private Shader outlineBufferShader;

    [SerializeField]
    private RenderTexture preOutlineBuffer;

    [SerializeField]
    private LayerMask layerMask;

    private LayerMask outlineLayerMask;
    private Canvas uiCanvas;

    public Material outlineMat;

    public bool saveImg = false;

    void Awake ()
    {
        cam = GetComponent<Camera>();

        cam.depthTextureMode = DepthTextureMode.Depth;
        outlineLayerMask = layerMask;

        outlineBufferCam = new GameObject().AddComponent<Camera>();
        outlineBufferCam.gameObject.name = "OutlineCamera";

        outlineBufferCam.CopyFrom(cam);
        outlineBufferCam.transform.parent = transform;
        outlineBufferCam.clearFlags = CameraClearFlags.Color;
        outlineBufferCam.backgroundColor = Color.black;
        outlineBufferCam.cullingMask = 1 << LayerMask.NameToLayer("HeatEmission");
        outlineBufferCam.renderingPath = RenderingPath.Forward;
        outlineBufferCam.depthTextureMode = DepthTextureMode.None;
        outlineBufferCam.enabled = false;

        UpdateOutlineCameraSettings();
    }

    void UpdateOutlineCameraSettings()
    {
        // Might have to put other camera properties that change in here too
        outlineBufferCam.fieldOfView = cam.fieldOfView;
    }

    void Update()
    {
        GenerateBufferIfNecessary(ref preOutlineBuffer, 0, RenderTextureFormat.ARGB32, "Pre-outline buffer");

        UpdateOutlineCameraSettings();
    }

    void GenerateBufferIfNecessary(ref RenderTexture buffer, int depth, RenderTextureFormat format, string name)
    {
        if (!buffer || buffer.width != Screen.width || buffer.height != Screen.height)
        {
            buffer = new RenderTexture(Screen.width, Screen.height, depth, format);
            buffer.name = name;
            buffer.Create();
        }
    }

    void OnRenderImage (RenderTexture source, RenderTexture destination)
    {
        outlineBufferCam.cullingMask = layerMask;
        outlineBufferCam.SetTargetBuffers(preOutlineBuffer.colorBuffer, preOutlineBuffer.depthBuffer);
        outlineBufferCam.RenderWithShader(outlineBufferShader, null);

        if (saveImg)
        {
            Texture2D newTexture = new Texture2D(preOutlineBuffer.width, preOutlineBuffer.height, TextureFormat.ARGB32, false);

            RenderTexture.active = preOutlineBuffer;
            newTexture.ReadPixels(new Rect(0, 0, preOutlineBuffer.width, preOutlineBuffer.height), 0, 0);

            byte[] bytes = newTexture.EncodeToPNG();
            System.IO.File.WriteAllBytes(Application.dataPath + "/../testscreen.png", bytes);
            Debug.Log(Application.dataPath + "/../testscreen.png", this);

            //Tell unity to delete the texture, by default it seems to keep hold of it and memory crashes will occur after too many screenshots.
            RenderTexture.active = null;
            Destroy(newTexture);
            saveImg = false;
        }

        outlineMat.SetTexture("_SceneTex", source);
        Graphics.Blit(preOutlineBuffer, destination, outlineMat);
    }

}