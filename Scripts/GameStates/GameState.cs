using UnityEngine;

public class GameState : IGameState
{
    public void EnterState(GameManager gameManager)
    {
        Debug.Log("Entered Game Mode");
    }

    public void UpdateState(GameManager gameManager)
    {
    }

    public void ExitState(GameManager gameManager)
    {
        Debug.Log("Exiting Game Mode");
    }
}
