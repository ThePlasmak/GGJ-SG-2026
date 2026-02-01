using UnityEngine;
using UnityEditor;

public class TextureColorFixer
{
    [MenuItem("Tools/Fix Texture Colors")]
    public static void FixColors()
    {
        SimpleTextureGenerator[] generator = Object.FindObjectsOfType<SimpleTextureGenerator>();
        foreach(var gen in generator)
        {
            SerializedObject so = new SerializedObject(gen);
            so.FindProperty("colorLeft").colorValue = new Color(0.9f, 0.0f, 0.5f);
            so.FindProperty("colorCenter").colorValue = new Color(1.0f, 0.6f, 0.8f);
            so.FindProperty("colorRight").colorValue = new Color(0.9f, 0.0f, 0.5f);
            so.ApplyModifiedProperties();
            
            gen.GenerateTexture();
            Debug.Log($"Updated colors for {gen.name}");
        }
    }
}
