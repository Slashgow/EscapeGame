using UnityEngine;

public class TextureSwapper : MonoBehaviour
{
    [SerializeField] private Material targetMaterial; 
    [SerializeField] private Texture[] textures; 
    private int currentIndex = 0;

    private void Awake()
    {
        targetMaterial.SetTexture("_BaseMap", textures[currentIndex]);
    }

    public void SwapTexture()
    {
        if (textures.Length == 0 || targetMaterial == null)
            return;

        currentIndex = (currentIndex + 1) % textures.Length;
        targetMaterial.SetTexture("_BaseMap", textures[currentIndex]); 
    }

}
