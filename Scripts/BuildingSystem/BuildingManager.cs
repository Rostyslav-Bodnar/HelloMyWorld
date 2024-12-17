using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    [SerializeField] private WorldGrid worldGrid;
    [SerializeField] private Building currentBuilding;
    [SerializeField] private CursorController cursorController;
    [SerializeField] private StorageUI storageUI;

    [SerializeField] public GameObject buildingPanel;

    public GameObject buildingPreview; 

    private bool canPlaceBuilding;

    public void HandleBuildingInput()
    {
        if (currentBuilding != null)
        {
            Vector3Int grid = cursorController.GetCursorPosition();
            Vector2Int gridCoord = new Vector2Int(grid.x, grid.z);

            canPlaceBuilding = CanPlaceBuilding(gridCoord, currentBuilding, worldGrid.grid, worldGrid.size);

            if (buildingPreview != null)
            {
                buildingPreview.transform.position = new Vector3(gridCoord.x, 0, gridCoord.y);
                cursorController.cursor.transform.position = new Vector3(gridCoord.x, 0, gridCoord.y);
            }
            MeshRenderer meshRenderer = buildingPreview.GetComponent<MeshRenderer>();

            meshRenderer.material.color = canPlaceBuilding ? Color.green : Color.red;

            if (Input.GetMouseButtonDown(1) && canPlaceBuilding)
            {
                PlaceBuilding(gridCoord, currentBuilding, worldGrid.grid, worldGrid.size);
                cursorController.cursor.SetActive(false);
            }
        }
    }

    private bool CanPlaceBuilding(Vector2Int gridCoord, Building building, Cell[,] grid, int size)
    {
        if (gridCoord.x + building.width > size || gridCoord.y + building.height > size)
            return false;
        Bounds buildingBounds = cursorController.cursor.GetComponent<Collider>().bounds;
        if (Physics.CheckBox(buildingBounds.center, buildingBounds.extents / 2, Quaternion.identity, LayerMask.NameToLayer("Resource")))
        {
            Debug.Log("Building touches another collider.");
            return false;
        }
        for (int y = gridCoord.y; y < gridCoord.y + building.height; y++)
        {
            for (int x = gridCoord.x; x < gridCoord.x + building.width; x++)
            {
                if (grid[x, y].isWater || grid[x, y].isOcuppied)
                {
                    if (grid[x, y].isWater)
                        Debug.Log("Water");
                    if (grid[x, y].isOcuppied)
                        Debug.Log("Occupied");
                    return false;
                }
            }
        }
        return true;
    }

    private void PlaceBuilding(Vector2Int gridCoord, Building building, Cell[,] grid, int size)
    {
        if (CanPlaceBuilding(gridCoord, building, grid, size))
        {
            Instantiate(building.prefab, new Vector3(gridCoord.x, 0, gridCoord.y), Quaternion.identity, transform);

            for (int y = gridCoord.y; y < gridCoord.y + building.height; y++)
            {
                for (int x = gridCoord.x; x < gridCoord.x + building.width; x++)
                {
                    grid[x, y].isOcuppied = true;
                }
            }

            Destroy(buildingPreview);
            currentBuilding = null;
        }
        else
        {
            Debug.Log("Cannot place building here.");
        }
    }

    public void SetCurrentBuilding(Building building)
    {
        currentBuilding = building;
        if (buildingPreview != null)
        {
            Destroy(buildingPreview);
        }
        if (currentBuilding == null)
            return;
        buildingPreview = Instantiate(currentBuilding.prefab, transform);
        cursorController.cursor.SetActive(true);
        cursorController.SetCursorScale(currentBuilding.width, currentBuilding.height);
    }

    public void ResetCurrentBuilding()
    {
        currentBuilding = null;
        if(buildingPreview != null)
            Destroy(buildingPreview);
    }
}
