using UnityEngine;

public class SimpleTextureGenerator : MonoBehaviour
{
    [SerializeField] private int textureSize = 24; // Multiple of 3 for equal lanes
    [Header("Lane Colors")]
    [SerializeField] private Color colorLeft = Color.red;
    [SerializeField] private Color colorCenter = Color.yellow;
    [SerializeField] private Color colorRight = Color.blue;
    
    private void Start()
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
