using UnityEngine;

public class HeatImaging : MonoBehaviour
{
    private Camera cam;
    private Camera outlineBufferCam;
    private Shader outlineBufferShader;

    [SerializeField]
    private RenderTexture preOutlineBuffer;

    [SerializeField]
    private RenderTexture preOutlineDepthBuffer;

    [SerializeField]
    private LayerMask layerMask;

    private LayerMask outlineLayerMask;
    private Canvas uiCanvas;

    public Material outlineMat;

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
        outlineBufferCam.cullingMask = 1 << LayerMask.NameToLayer("Outline");
        outlineBufferCam.renderingPath = RenderingPath.Forward;
        outlineBufferCam.depthTextureMode = DepthTextureMode.None;
        outlineBufferCam.enabled = false;

        outlineBufferShader = Shader.Find("Custom/OutlineBuffer");

        UpdateOutlineCameraSettings();
    }

    void UpdateOutlineCameraSettings()
    {
        // Might have to put other camera properties that change in here too
        outlineBufferCam.fieldOfView = cam.fieldOfView;
    }

    void Update()
    {
        GenerateBufferIfNecessary(ref preOutlineBuffer, 0, RenderTextureFormat.R8, "Pre-outline buffer");
        GenerateBufferIfNecessary(ref preOutlineDepthBuffer, 0, RenderTextureFormat.Depth, "Pre-outline depth buffer");

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
        outlineBufferCam.SetTargetBuffers(preOutlineBuffer.colorBuffer, preOutlineDepthBuffer.depthBuffer);
        outlineBufferCam.RenderWithShader(outlineBufferShader, null);

        outlineMat.SetTexture("_SceneTex", source);
        outlineMat.SetTexture("_OutlineDepthBuffer", preOutlineDepthBuffer);
        Graphics.Blit(preOutlineBuffer, destination, outlineMat);
    }

}