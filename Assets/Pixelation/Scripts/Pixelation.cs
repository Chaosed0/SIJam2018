using Assets.Pixelation.Example.Scripts;
using UnityEngine;

namespace Assets.Pixelation.Scripts
{
    [ExecuteInEditMode]
    [AddComponentMenu("Image Effects/Color Adjustments/Pixelation")]
    public class Pixelation : ImageEffectBase
    {
        public float BlockCount = 128;
        public float ColorStep = 64;

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            float k = Camera.main.aspect;
            Vector2 count = new Vector2(BlockCount, BlockCount/k);
            material.SetVector("BlockCount", count);
            material.SetFloat("ColorStep", ColorStep);
            Graphics.Blit(source, destination, material);
        }
    }
}