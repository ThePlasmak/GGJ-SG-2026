using UnityEngine;

public class HallwayBackground : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float hallwayWidth = 15f; // Matches 3 lanes of 5 width
    [SerializeField] private float hallwayHeight = 8f;
    [SerializeField] private float hallwayLength = 100f;
    [SerializeField] private float extraDepth = 50f; // Extra space behind the opponent spawn
    [SerializeField] private Color leftWallColor = new Color(0.9f, 0.0f, 0.5f); // Hot Pink
    [SerializeField] private Color rightWallColor = new Color(0.9f, 0.0f, 0.5f); // Hot Pink
    [SerializeField] private Color ceilingColor = new Color(1.0f, 0.6f, 0.8f); // Light Pink
    // End wall will match Ceiling for now, or use a new color if requested.
    
    private GameObject wallsRoot;
    private Renderer[] wallRenderers;

    // ...

    private void GenerateWalls()
    {
        if (wallsRoot != null) Destroy(wallsRoot);

        wallsRoot = new GameObject("HallwayWalls_Generated");
        wallsRoot.transform.SetParent(transform);
        wallsRoot.transform.localPosition = Vector3.zero;

        Material CreateMat(Color c) {
            Material m = new Material(Shader.Find("Standard"));
            m.color = c;
            m.SetFloat("_Glossiness", 0.0f);
            return m;
        }

        // Calculate geometry with extra depth
        // We want to keep the 'start' (near camera) at -Length/2
        // And extend the 'end' (far) to +Length/2 + ExtraDepth
        float totalLength = hallwayLength + extraDepth;
        float zCenterOffset = extraDepth * 0.5f;

        // Left Wall
        CreateWall(new Vector3(-hallwayWidth * 0.5f, hallwayHeight * 0.5f, zCenterOffset), 
                   new Vector3(1, hallwayHeight, totalLength), 
                   CreateMat(leftWallColor), "LeftWall");

        // Right Wall
        CreateWall(new Vector3(hallwayWidth * 0.5f, hallwayHeight * 0.5f, zCenterOffset), 
                   new Vector3(1, hallwayHeight, totalLength), 
                   CreateMat(rightWallColor), "RightWall");

        // Ceiling
        CreateWall(new Vector3(0, hallwayHeight, zCenterOffset), 
                   new Vector3(hallwayWidth, 1, totalLength), 
                   CreateMat(ceilingColor), "Ceiling");
                   
        // End Wall (Far Z)
        // Position: X=0, Y=Height/2, Z=(Length/2) + ExtraDepth
        CreateWall(new Vector3(0, hallwayHeight * 0.5f, (hallwayLength * 0.5f) + extraDepth),
                   new Vector3(hallwayWidth, hallwayHeight, 1),
                   CreateMat(ceilingColor), "EndWall");
                   
        // Store renderers if we want to animate them later
        wallRenderers = wallsRoot.GetComponentsInChildren<Renderer>();
    }

    private void CreateWall(Vector3 localPos, Vector3 scale, Material mat, string name)
    {
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.name = name;
        wall.transform.SetParent(wallsRoot.transform);
        wall.transform.localPosition = localPos;
        wall.transform.localScale = scale;
        
        Renderer r = wall.GetComponent<Renderer>();
        if (r != null) r.sharedMaterial = mat;
        
        // Disable collider so it doesn't mess with physics
        Collider c = wall.GetComponent<Collider>();
        if (c != null) Destroy(c);
    }
}
