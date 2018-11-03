using Assets.Pixelation.Example.Scripts;
using UnityEngine;

[ExecuteInEditMode]
public class Blue : ImageEffectBase
{
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, material);
    }
}
