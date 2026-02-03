using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
/// <summary>
/// This class controls the movement of a cart in a VR environment based on the input from left and right hand controllers.
/// It ensures that the cart moves only when both handles are grabbed and the required preconditions (such as power being on and park button off) are met.
/// The class uses Rigidbody physics to apply movement forces based on the difference in controller positions. It also tracks docking states and manages the activation of related button lights when the cart reaches certain targets.
/// </summary>
public class CartMovementController : MonoBehaviour
{
    // References to the left and right hand controllers' transforms
    public Transform leftControllerTransform;
    public Transform rightControllerTransform;

    // References to the grab interactions on the cart's handles
    public HandleXRGrabInteraction leftHandle;
    public HandleXRGrabInteraction rightHandle;
    // The Rigidbody component of the cart, used for physics-based movement
    public Rigidbody cartRigidbody;
    // References to control buttons
    public ToggleLightButton powerButton; // Power must be ON for movement
    public ToggleLightButton parkButton; // Park must be OFF for movement
    public BasicLightButton dockButton;  // Indicates when the cart is docked
    // Stores the last recorded positions of the controllers
    private Vector3 lastLeftPos;
    private Vector3 lastRightPos;
    private bool initialized = false; // Tracks if positions have been initialized
    // Force multiplier for movement speed
    public float forceMultiplier = 4000f;
    public Collider self;
    public Collider mriArea; // Reference to the MRI area collider
    private bool hasEnteredMriArea = false; // Tracks if the cart has entered the MRI area

    /// <summary>
    /// Called once per frame.
    /// - Checks if both handles are grabbed and if movement conditions are met.
    /// - Calls MoveCart() if conditions allow movement.
    /// - Resets initialization when handles are released.
    /// </summary>
    /// 
    private void Update()
    {
        bool leftGrabbed = leftHandle.isHandleGrabbed();
        bool rightGrabbed = rightHandle.isHandleGrabbed();
        // Ensure both handles are grabbed and movement conditions are met
        if (leftGrabbed && rightGrabbed && checkMovePreconditions())
        {
            MoveCart();
        }
        else
        {
            initialized = false; // Reset when handles are released
        }
    }
    /// <summary>
    /// Moves the cart based on the difference in position between the current and previous frames.
    /// - Calculates movement based on the average controller movement.
    /// - Moves the cart using Rigidbody for smooth physics-based motion.
    /// - Prevents unwanted vertical and sideways motion.
    /// </summary>
    private void MoveCart()
    {
        // Get the current positions of the controllers
        Vector3 leftCurrentPos = leftControllerTransform.position;
        Vector3 rightCurrentPos = rightControllerTransform.position;

        // Initialize previous positions on first movement
        if (!initialized)
        {
            lastLeftPos = leftCurrentPos;
            lastRightPos = rightCurrentPos;
            initialized = true;
            return; // Skip first frame to avoid sudden jump
        }

        // Calculate movement direction by averaging both controller movements
        Vector3 leftMovement = leftCurrentPos - lastLeftPos;
        Vector3 rightMovement = rightCurrentPos - lastRightPos;
        Vector3 averageMovement = (leftMovement + rightMovement) / 2f;

        // Convert movement to cart's local space (so it moves forward/backward)
        Vector3 localMovement = transform.InverseTransformDirection(averageMovement);
        localMovement.y = 0f;  // Prevent vertical movement
        localMovement.z = 0f;  // Only move forward/backward

        // Apply direct position movement (instead of force)
        Vector3 newPosition = cartRigidbody.position + transform.TransformDirection(localMovement) * forceMultiplier * Time.fixedDeltaTime;
        cartRigidbody.MovePosition(newPosition);

        // Store last positions for next frame
        lastLeftPos = leftCurrentPos;
        lastRightPos = rightCurrentPos;
    }

    /// <summary>
    /// Checks whether movement conditions are met.
    /// - The power button must be ON.
    /// - The park button must be OFF.
    /// </summary>
    /// <returns>True if movement conditions are met, false otherwise.</returns>
    private bool checkMovePreconditions()
    {
        return powerButton.getState() && !parkButton.getState();
    }

    /// <summary>
    /// Checks whether the cart is currently docked.
    /// </summary>
    /// <returns>True if the cart is docked, false otherwise.</returns>
    public bool isDocked()
    {
        return dockButton.getState();
    }
    /// <summary>
    /// Handles the event when the cart enters a trigger zone.
    /// - If the cart reaches the MRI area and the park button is OFF, it docks automatically.
    /// - Turns on the dock button and activates the park button to prevent further movement.
    /// </summary>
    /// <param name="other">The collider that the cart has entered.</param>
    private void OnTriggerEnter(Collider other)
    {
        // Check if the cart's collider enters the MRI area and hasn't already entered
        if (!hasEnteredMriArea && self.bounds.Intersects(mriArea.bounds) && !parkButton.getState())
        {
            dockButton.TurnOn();
            parkButton.TurnOn();
            hasEnteredMriArea = true; // Mark that the cart has entered the MRI area
        }
    }

    /// <summary>
    /// Handles the event when the cart exits a trigger zone.
    /// - If the cart leaves the MRI area, the dock indicator is turned off.
    /// </summary>
    /// <param name="other">The collider that the cart has exited.</param>
    private void OnTriggerExit(Collider other)
    {
        // Only trigger exit if the cart has already entered the MRI area and now leaves it
        if (hasEnteredMriArea && !self.bounds.Intersects(mriArea.bounds))
        {
            dockButton.TurnOff();
            hasEnteredMriArea = false; // Mark that the cart has exited the MRI area
        }
    }

}
