using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class WorldGrid : MonoBehaviour
{
    [SerializeField] private Material terrainMaterial;
    [SerializeField] private Material edgeMaterial;

    public float waterLevel = .2f;
    public float scale = .001f;
    public int size = 500;

    public Cell[,] grid;

    public void GenerateWorld()
    {
        GenerateGrid();
        DrawTerrainMesh(grid);
        DrawEdgeMesh(grid);
        DrawTexture(grid);
    }

    private void GenerateGrid()
    {
        float[,] noiseMap = new float[size, size];
        float xOffset = Random.Range(-10000f, 10000f);
        float yOffset = Random.Range(-10000f, 10000f);
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float noiseValue = Mathf.PerlinNoise(x * scale + xOffset, y * scale + yOffset);
                noiseMap[x, y] = noiseValue;
            }
        }

        float[,] fallOffMap = new float[size, size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float xv = x / (float)size * 2 - 1;
                float yv = y / (float)size * 2 - 1;
                float v = Mathf.Max(Mathf.Abs(xv), Mathf.Abs(yv));
                fallOffMap[x, y] = Mathf.Pow(v, 3f) / (Mathf.Pow(v, 3f) + Mathf.Pow(2.2f - 2.2f * v, 3f));
            }
        }

        grid = new Cell[size, size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = new Cell();
                float noiseValue = noiseMap[x, y];
                noiseValue -= fallOffMap[x, y];
                cell.isWater = noiseValue < waterLevel;
                grid[x, y] = cell;

            }
        }
    }
    private void DrawTerrainMesh(Cell[,] grid)
    {
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();
        for (int y = 0; y < size; y++)
        {
            for (int x = 0;x < size; x++)
            {
                Cell cell = grid[x, y];
                if(!cell.isWater)
                {
                    Vector3 a = new Vector3(x, 0, y);
                    Vector3 b = new Vector3(x, 0, y + 1);
                    Vector3 c = new Vector3(x + 1, 0, y);
                    Vector3 d = new Vector3(x + 1, 0, y + 1);

                    Vector2 uvA = new Vector2(x / (float)size, y / (float)size);
                    Vector2 uvB = new Vector2(x / (float)size, (y + 1) / (float)size);
                    Vector2 uvC = new Vector2((x + 1) / (float)size, y / (float)size);
                    Vector2 uvD = new Vector2((x + 1) / (float)size, (y + 1) / (float)size);
                    Vector3[] v = new Vector3[] {a, b, c, b, d, c };
                    Vector2[] uv = new Vector2[] {uvA, uvB, uvC, uvB, uvC, uvD};
                    for (int k = 0; k < v.Length; k++)
                    {
                        vertices.Add(v[k]);
                        triangles.Add(triangles.Count);
                        uvs.Add(uv[k]);
                    }

                }
            }
        }
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();

        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        //Collider collider = gameObject.AddComponent<MeshCollider>();

    }
    private void DrawTexture(Cell[,] grid)
    {
        Texture2D texture = new Texture2D(size, size);
        Color[] colorMap = new Color[size * size];
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = grid[x, y];
                if (cell.isWater)
                    colorMap[y * size + x] = Color.blue;
                else
                    colorMap[y * size + x] = Color.green;
                
            }
        }
        texture.filterMode = FilterMode.Point;
        texture.SetPixels(colorMap);
        texture.Apply();

        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        meshRenderer.material = terrainMaterial;
        meshRenderer.material.mainTexture = texture;

        BoxCollider collider = gameObject.AddComponent<BoxCollider>();
    }
    private void DrawEdgeMesh(Cell[,] grid)
    {
        Mesh mesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Cell cell = grid[x, y];
                if (!cell.isWater)
                {
                    if (x > 0)
                    {
                        Cell left = grid[x - 1, y];
                        if (left.isWater)
                        {
                            Vector3 a = new Vector3(x, 0, y + 1);
                            Vector3 b = new Vector3(x, 0, y);
                            Vector3 c = new Vector3(x, -1, y + 1);
                            Vector3 d = new Vector3(x, -1, y);
                            Vector3[] v = new Vector3[] { a, b, c, b, d, c };
                            for (int k = 0; k < 6; k++)
                            {
                                vertices.Add(v[k]);
                                triangles.Add(triangles.Count);
                            }

                        }
                    }
                    if (x < size - 1)
                    {
                        Cell right = grid[x + 1, y];
                        if (right.isWater)
                        {
                            Vector3 a = new Vector3(x + 1, 0, y);
                            Vector3 b = new Vector3(x + 1, 0, y + 1);
                            Vector3 c = new Vector3(x + 1, -1, y);
                            Vector3 d = new Vector3(x + 1, -1, y + 1);
                            Vector3[] v = new Vector3[] { a, b, c, b, d, c };
                            for (int k = 0; k < 6; k++)
                            {
                                vertices.Add(v[k]);
                                triangles.Add(triangles.Count);
                            }

                        }
                    }
                    if(y > 0)
                    {
                        Cell down = grid[x, y - 1];
                        if (down.isWater)
                        {
                            Vector3 a = new Vector3(x, 0, y);
                            Vector3 b = new Vector3(x + 1, 0, y);
                            Vector3 c = new Vector3(x, -1, y);
                            Vector3 d = new Vector3(x + 1, -1, y);
                            Vector3[] v = new Vector3[] { a, b, c, b, d, c };
                            for (int k = 0; k < 6; k++)
                            {
                                vertices.Add(v[k]);
                                triangles.Add(triangles.Count);
                            }

                        }
                    }
                    if (y < size - 1)
                    {
                        Cell up = grid[x, y + 1];
                        if (up.isWater)
                        {
                            Vector3 a = new Vector3(x + 1, 0, y + 1);
                            Vector3 b = new Vector3(x, 0, y + 1);
                            Vector3 c = new Vector3(x + 1, -1, y + 1);
                            Vector3 d = new Vector3(x, -1, y + 1);
                            Vector3[] v = new Vector3[] { a, b, c, b, d, c };
                            for (int k = 0; k < 6; k++)
                            {
                                vertices.Add(v[k]);
                                triangles.Add(triangles.Count);
                            }

                        }
                    }
                }
                
                
            }
        }
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        GameObject edgeObg = new GameObject("Edge");
        edgeObg.transform.SetParent(transform);
        edgeObg.transform.localPosition = Vector3.zero;

        MeshFilter meshFilter = edgeObg.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        MeshRenderer meshRenderer = edgeObg.AddComponent<MeshRenderer>();
        meshRenderer.material = edgeMaterial;

    }
}
