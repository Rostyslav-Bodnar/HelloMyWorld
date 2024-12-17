public interface IGameState
{
    void EnterState(GameManager gameManager);
    void UpdateState(GameManager gameManager);
    void ExitState(GameManager gameManager);
}
