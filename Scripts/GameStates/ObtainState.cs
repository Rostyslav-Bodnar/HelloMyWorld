using UnityEngine;

public class ObtainState : IGameState
{
    private CursorController cursorController;

    public ObtainState(CursorController cursorController)
    {
        this.cursorController = cursorController;
    }

    public void EnterState(GameManager gameManager)
    {
        Debug.Log("Entered Obtain Mode");
        cursorController.cursor.SetActive(true);
    }

    public void UpdateState(GameManager gameManager)
    {
        cursorController.HandleCursorInput();
    }

    public void ExitState(GameManager gameManager)
    {
        Debug.Log("Exiting Obtain Mode");
        cursorController.cursor.SetActive(false);
    }
}
