using Unity.AI.Navigation;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private IGameState _currentState;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private BuildingManager buildingManager;
    [SerializeField] private WorldGrid worldGrid;
    [SerializeField] private ResourceGenerator resourceGenerator;
    [SerializeField] private CursorController cursorController;

    [SerializeField] private NavMeshSurface nav;

    private void Start()
    {
        GenerateWorld();
        SetState(new GameState());
        nav.BuildNavMesh();
    }

    private void GenerateWorld()
    {
        worldGrid.GenerateWorld();
        resourceGenerator.GenerateResources(worldGrid.grid, worldGrid.size);
    }

    private void Update()
    {
        _currentState?.UpdateState(this);
    }

    public void SetState(IGameState newState)
    {
        _currentState?.ExitState(this);
        _currentState = newState;
        _currentState.EnterState(this);
    }

    public void SetGameMode() => SetState(new GameState());
    public void SetObtainMode() => SetState(new ObtainState(cursorController));
    public void SetBuildMode() => SetState(new BuildState(buildingManager, cursorController));
}