public static class InputManager
{
    public static readonly PlayerInputActions PlayerInputActions;
    static InputManager()
    {
        PlayerInputActions = new PlayerInputActions();
        PlayerInputActions.Enable();
    }
}