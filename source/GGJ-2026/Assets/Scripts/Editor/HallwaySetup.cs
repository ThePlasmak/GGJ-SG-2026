using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class HallwaySetup : MonoBehaviour
{
    [MenuItem("Tools/Setup Hallway Opponent")]
    public static void SetupOpponent()
    {
        // 1. Find the Opponent
        HallwayOpponent opponent = FindObjectOfType<HallwayOpponent>();
        if (opponent == null)
        {
            Debug.LogError("Could not find a HallwayOpponent in the scene!");
            return;
        }

        // 2. Find Specific Sprites
        string bodyPath = "Assets/Art/DontCollide/DontCollide_ObstacleBody.png";
        string eyesPath = "Assets/Art/DontCollide/DontCollide_ObstacleEyes.png";
        
        Sprite bodySprite = LoadFirstSprite(bodyPath);
        Sprite eyesSprite = LoadFirstSprite(eyesPath);

        if (bodySprite == null || eyesSprite == null)
        {
            Debug.LogError($"Could not find sprites! Body: {bodySprite}, Eyes: {eyesSprite}. Check paths and Import Settings.");
            return;
        }

        // 3. Create/Find Child Objects
        SpriteRenderer bodyRenderer = CreateOrGetChildRenderer(opponent.transform, "BodySprite");
        SpriteRenderer eyesRenderer = CreateOrGetChildRenderer(opponent.transform, "EyesSprite");

        // 4. Assign to Script
        SerializedObject so = new SerializedObject(opponent);
        so.FindProperty("bodyRenderer").objectReferenceValue = bodyRenderer;
        so.FindProperty("eyesRenderer").objectReferenceValue = eyesRenderer;
        so.FindProperty("bodySprite").objectReferenceValue = bodySprite;
        so.FindProperty("eyesSprite").objectReferenceValue = eyesSprite;
        so.ApplyModifiedProperties();

        // 5. Setup Renderers
        bodyRenderer.sprite = bodySprite;
        eyesRenderer.sprite = eyesSprite;
        
        // Sorting Order
        bodyRenderer.sortingOrder = 0;
        eyesRenderer.sortingOrder = 1; // Eyes on top

        Debug.Log("Successfully Setup Hallway Opponent with Body and Eyes!");
    }

    private static Sprite LoadFirstSprite(string path)
    {
        // Try simple load first (Single Mode)
        Sprite s = AssetDatabase.LoadAssetAtPath<Sprite>(path);
        if (s != null) return s;

        // Try loading all (Multiple Mode)
        Object[] assets = AssetDatabase.LoadAllAssetsAtPath(path);
        foreach (Object obj in assets)
        {
            if (obj is Sprite sprite)
            {
                return sprite;
            }
        }
        return null;
    }

    private static SpriteRenderer CreateOrGetChildRenderer(Transform parent, string name)
    {
        Transform t = parent.Find(name);
        if (t == null)
        {
            GameObject go = new GameObject(name);
            t = go.transform;
            t.SetParent(parent);
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
        }
        
        SpriteRenderer sr = t.GetComponent<SpriteRenderer>();
        if (sr == null) sr = t.gameObject.AddComponent<SpriteRenderer>();
        return sr;
    }
}
