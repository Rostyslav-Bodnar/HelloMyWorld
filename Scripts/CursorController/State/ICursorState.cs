public interface ICursorState
{
    void EnterState(CursorController cursorController);
    void UpdateState(CursorController cursorController);
    void ExitState(CursorController cursorController);
}
