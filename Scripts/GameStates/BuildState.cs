using UnityEngine;

public class BuildState : IGameState
{
    private BuildingManager _buildingManager;
    private CursorController _cursorController;

    public BuildState(BuildingManager buildingManager, CursorController cursorController)
    {
        _buildingManager = buildingManager;
        _cursorController = cursorController;
    }

    public void EnterState(GameManager gameManager)
    {
        Debug.Log("Entered Build Mode");
        _cursorController.cursor.SetActive(true);
        _buildingManager.buildingPanel.SetActive(true);
    }

    public void UpdateState(GameManager gameManager)
    {
        _buildingManager.HandleBuildingInput();
    }

    public void ExitState(GameManager gameManager)
    {
        Debug.Log("Exiting Build Mode");
        _cursorController.cursor.SetActive(false);
        _buildingManager.buildingPanel.SetActive(false);
        _cursorController.SetCursorScale(1f, 1f);
        _buildingManager.ResetCurrentBuilding();

    }

}
