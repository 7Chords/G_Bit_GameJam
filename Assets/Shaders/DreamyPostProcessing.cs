using UnityEngine;

public class DreamyPostProcessing : MonoBehaviour
{
    public Material dreamyMaterial;
    public float distortion = 0.5f;
    public float blurSize = 1.0f;

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (dreamyMaterial != null)
        {
            dreamyMaterial.SetFloat("_Distortion", distortion);
            dreamyMaterial.SetFloat("_BlurSize", blurSize);
            Graphics.Blit(source, destination, dreamyMaterial);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }
}
