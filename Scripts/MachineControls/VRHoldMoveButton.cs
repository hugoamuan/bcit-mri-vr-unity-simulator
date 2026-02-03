using UnityEngine;

/// <summary>
/// Extends <see cref="VRHoldButton"/> to handle movement controls for a bed.
/// Determines movement direction (up or down) based on the button type.
/// </summary>
public class VRHoldMoveButton : VRHoldButton
{
    private bool moveUp;

    void Awake()
    {
        moveUp = type == "Up";
    }

    /// <summary>
    /// Overrides behavior when the button is held:
    /// - Moves the bed in the corresponding direction.
    /// - Turns on the button light.
    /// </summary>
    protected override void ifHeld()
    {
        base.ifHeld(); // Keep the default light behavior
        Move();
    }

    private void Move()
    {
        if (bedController == null) return;

        if (moveUp)
        {
            bedController.MoveUp();
        }
        else
        {
            bedController.MoveDown();
        }
    }
}
