using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    [SerializeField] private Resource[] resources;

    public void GenerateResources(Cell[,] grid, int size)
    {
        for (int i = 0; i < resources.Length; i++)
        {
            Resource resource = resources[i];
            float[,] noiseMap = new float[size, size];
            float xOffset = Random.Range(-10000f, 10000f);
            float yOffset = Random.Range(-10000f, 10000f);
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    float noiseValue = Mathf.PerlinNoise(x * resource.noiseScale + xOffset, y * resource.noiseScale + yOffset);
                    noiseMap[x, y] = noiseValue;
                }
            }

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    Cell cell = grid[x, y];
                    if (!cell.isWater)
                    {
                        float v = Random.Range(0f, resource.dencity);
                        if (noiseMap[x, y] < v)
                        {
                            if (resource is OvergroundedResource overgroundedResource && !cell.isOcuppied)
                            {
                                GameObject stone = Instantiate(overgroundedResource.prefab, transform);
                                stone.transform.position = new Vector3(x + 0.5f, 0, y + 0.5f);
                                cell.resource = overgroundedResource;
                                stone.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);
                                stone.transform.localScale = Vector3.one * Random.Range(.8f, 1.2f);
                                cell.isOcuppied = true;
                            }
                            else if (resource is UndergroundResource undergroundResource)
                            {
                                Debug.Log("Resource is Undergrounded");
                            }

                        }
                    }
                }
            }
        }
    }

}
