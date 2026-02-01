using UnityEngine;

[ExecuteAlways]
public class SimpleTextureGenerator : MonoBehaviour
{
    [SerializeField] private int textureSize = 24; // Multiple of 3 for equal lanes
    [Header("Lane Colors")]
    [SerializeField] private Color colorLeft = new Color(0.9f, 0.0f, 0.5f); // Hot Pink
    [SerializeField] private Color colorCenter = new Color(1.0f, 0.6f, 0.8f); // Light Pink
    [SerializeField] private Color colorRight = new Color(0.9f, 0.0f, 0.5f); // Hot Pink
    
    private void Start()
    {
        GenerateTexture();
    }

    private void OnValidate()
    {
        // Delay to avoid sendmessage errors
        // Or just ensure we are safe. Texture generation is safe.
        // But OnValidate shouldn't create excessive garbage.
        // We'll call GenerateTexture only if we have a renderer.
    }
    
    [ContextMenu("Force Regenerate")]
    public void GenerateTexture()
    {
        Renderer ren = GetComponent<Renderer>();
        if (ren == null) return;

        Texture2D texture = new Texture2D(textureSize, textureSize);
        texture.filterMode = FilterMode.Point; // Crisp edges

        for (int x = 0; x < textureSize; x++)
        {
            // 3 Lanes: 0-33%, 33-66%, 66-100%
            float u = (float)x / textureSize;
            
            Color pixelColor = colorCenter;
            
            if (u < 0.33f)
            {
                pixelColor = colorLeft;
            }
            else if (u > 0.66f)
            {
                pixelColor = colorRight;
            }
            else
            {
                pixelColor = colorCenter;
            }
            
            for (int y = 0; y < textureSize; y++)
            {
                texture.SetPixel(x, y, pixelColor);
            }
        }

        texture.Apply();
        
        texture.Apply();
        
        // Use Unlit shader so it glows/shows color regardless of lighting
        Material tempMaterial = new Material(Shader.Find("Unlit/Texture")); 
        if(tempMaterial.shader == null) tempMaterial = new Material(Shader.Find("Standard"));
        
        tempMaterial.mainTexture = texture;
        ren.sharedMaterial = tempMaterial;
        
        Debug.Log($"Texture Generated! Size: {textureSize}, Colors: {colorLeft}/{colorCenter}/{colorRight}");
    }
}
