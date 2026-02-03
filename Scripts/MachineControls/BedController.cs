using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class controls the movement and positioning of the bed, including the tray, table, and cart.
/// It interacts with the <see cref="TableController"/>, <see cref="TrayController"/>, and <see cref="CartMovementController"/> to move these components 
/// based on specific conditions, such as whether the tray is at its maximum height or whether the cart is docked.
/// Methods include moving the bed up or down, resetting to home position, and moving the tray and table to fixed distances or maximum positions.
/// </summary>
public class BedController : MonoBehaviour
{
    // Controls table movement
    public TableController tableController;
    // Controls tray movement
    public TrayController trayController;
    // Manages Cart movement
    public CartMovementController cartMovementController;

    /// <summary>
    // Moves the bed upwards
    // If the tray is not at the max height, it moves the tray and table up
    // If the tray is at the max height and the cart is docked, it moves the tray into the MRI
    // Otherwise, leave it
    // /// </summary>
    public void MoveUp()
    {
        // for moving up, if the table/tray is not at max height, move both tray and table up
        if (!tableController.IsAtMax())
        {
            tableController.MoveUp();
        }
        else if (!cartMovementController.isDocked())
        {
            // if the tray is at max height and the cart is not docked, do nothing
        }
        else // if the tray is at max height, move the tray into mri
        {
            trayController.MoveIn();
        }
    }

    /// <summary>
    /// Moves the bed downwards.
    /// - If the tray is outside the MRI, both the tray and table move down.
    /// - If the tray is inside the MRI, the tray moves out instead.
    /// </summary>
    public void MoveDown()
    {
        //if the tray is outside the mri, move tray and table down
        if (trayController.IsAtMin())
        {
            tableController.MoveDown();
        }
        else
        { // if the tray is inside the mri, move both tray out
            trayController.MoveOut();
        }
    }

    /// <summary>
    /// Moves the bed to its home position:
    /// - The table moves to max height.
    /// - The tray moves outside the MRI.
    /// </summary>
    public void HomePosition() // home position is max height for table and tray outside mri
    {
        tableController.MoveToMax();
        trayController.MoveToHome();
    }

    /// <summary>
    /// Moves the bed components to a fixed intermediate position.
    /// - Moves the table to max height.
    /// - Moves the tray to max height.
    /// - Moves the tray halfway into the MRI.
    /// </summary>
    public void MoveFixedDistance()
    {
        if (tableController.IsAtMax())
        {
            trayController.MoveToFixedDistance();
        }
        else
        {
            StartCoroutine(MoveFixedDistanceSequence());
        }
    }
    private IEnumerator MoveFixedDistanceSequence()
    {
        tableController.MoveToMax();

        while (!tableController.IsAtMax())
        {
            yield return null;
        }

        trayController.MoveToFixedDistance();
    }


    // Check if the tray is outside the MRI
    public bool IsAtMinX() // x is the horizontal axis into the MRI
    {
        return trayController.IsAtMin();
    }
}
