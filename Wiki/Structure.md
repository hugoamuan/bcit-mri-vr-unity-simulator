# Project Structure

This section outlines the organization of the **MRI VR Simulation** project.

---

## Root Directory  

The following files are located in the root directory of the project:

1. **.gitignore**  
   - Defines files and directories to be excluded from version control.

2. **.gitattributes**  
   - Defines attributes for files in the repository, such as handling large files.

---

## Assets Directory  

The **Assets** directory contains all assets used in the MRI VR Simulation. It is further organized as follows:

1. **Audio**  
   - Audio files for the simulation.

2. **Materials**  
   - Materials and shaders used for scene objects.

3. **Models**  
   - 3D models used in the simulation.

4. **Plugins**  
   - External plugins used in the project.

5. **Prefabs**  
   - Prefabricated Unity objects that can be reused in different scenes.

6. **Scenes**  
   - Unity scene files, including the main simulation scene.

7. **Scripts**  
   - C# scripts responsible for interactions, logic, and VR-specific functionality.

8. **TextMesh Pro**  
   - Text rendering assets.

9. **XR**  
   - Assets related to XR (Extended Reality) configurations and interactions.

10. **XRI**  
    - Specific files for XR Interaction Toolkit.

---

## Scripts Directory


### /Scripts
- **PatientWalkManager.cs**: Handles calling the patient in, walking them along a curved path, turn, and seating logic.
- **PatientPositionManager.cs**: Manages patient pose prefabs, dropdown, and exam-position correctness (implements CheckerInterface).
- **Project.Runtime.asmdef**: Assembly definition for the runtime scripts; references UI, TextMeshPro, Input System, and XR packages. Also used to connect to test scripts.
- **StartScanManager.cs**: Runs ErrorCheck before scan, then plays scanner audio and repurposes the feedback panel UI for “Patient Prep Results”.
- **Cables/**
	- **DumbCable.cs**: Simple sagging LineRenderer cable between two transforms (no collisions or physics).
	- **GrabTracker.cs**: Tracks XR grab/release on an interactable and flags which cable end is being held for PhysicsCablePlus.
	- **PhysicsCable.cs**: More advanced sagging cable with intermediate segments, smoothing, and ground collision raycasts.
	- **PhysicsCablePlus.cs**: Extends PhysicsCable with a “leash” that pulls the unheld rigidbody back when endpoints exceed a max distance.
- **Curtains/**
	- **PatientWalkIn.cs**: Trigger that, when the patient’s head target enters, fires the linked curtain’s SmoothCurtainAnimator once.
	- **SmoothCurtainAnimator.cs**: Smoothly animates curtain transform between home and target local position/scale, toggled via TriggerAnimation().
- **DoubleDoorTrigger/**
	- **TriggerDoorController.cs**: Controls Zone 4 door open/close animations; checkable (door closed) and used as a patient state condition.
	- **TriggerOrGrab.cs**: XRBaseInteractable that invokes a UnityEvent on either hover+trigger (left/right) or grab/select.
- **ErrorCheck/**
	- **BaseCheck.cs**: Abstract UI helper for error panels: builds error text, buttons, and exposes a Check() contract.
	- **CheckerInterface.cs**: Interface for “is this correct?” checks with a label for feedback reports.
	- **Container.cs**: Utility wrapper around a trigger collider to test if a Transform’s position lies within its bounds.
	- **DoubleDoor.cs**: Tracks player/patient crossing door colliders and runs room-prep and teardown Returned/Error checks and panels.
	- **ErrorCheck.cs**: Concrete BaseCheck, iterates error groups and reports per-object correctness via CheckerInterface.
	- **ErrorGroupEntry.cs**: Serializable mapping key → array of GameObjects used to define error groups for checks.
	- **HygieneButton.cs**: Simple click handler that notifies SanitizeHands when a hygiene button is pressed.
	- **PillowCheck.cs**
	- **RemovableHeadPlateStatus.cs**: ReturnedInterface implementation that is “returned” when it still has a child (head plate replaced).
	- **ReturnedCheck.cs**: BaseCheck variant that reports whether post-procedure actions were performed using ReturnedInterface.
	- **ReturnedInterface.cs**: Interface for “was this item/action returned/completed?” with a label for teardown feedback.
	- **SanitizeHands.cs**: Implements CheckerInterface; spawns cosmetic bubble effect and marks hand hygiene as done on button press.
	- **Zone3Door.cs**: Simple animator-driven toggle for the Zone 3 (computer room) door open/closed state.
- **ExamSelection/**
	- **PatientHoverParticleFeedback.cs**: XR interactable on the patient that plays hover/selection particles, sets exam type, updates dialogue, and advances patient state when confirmed.
- **GUI/**
	- **FloatingImageController.cs**:Shows/hides a world-space image in front of the camera and keeps it positioned and facing the player.
	- **ViveInputHandler.cs**: Listens to a Vive controller input action and toggles the FloatingImageController’s image on button press.
- **Hints/**
	- **HintArea.cs**: Registers a “hint zone” with HintManager and shows a highlight object when a matching item and exam type are active.
	- **HintManager.cs**: Singleton that tracks current held HoldableItem and exam type to show/hide the appropriate HintAreas.
	- **HoldableItem.cs**: Tag script for grabbable items that notifies HintManager when picked up or released with XR select events.
- **Indicator/**
	- **LabelToggle.cs**: Spawns a secondary camera that only renders the Labels layer and toggles it via a controller InputAction.
	- **ObjectLabel.cs**: Adds a TextMeshPro label above an object that follows it and always faces the main camera.
- **MachineControls/**
	- **BasicLightButton.cs**: Wraps a single indicator light; turns its ButtonLightBase entry on/off and mirrors state from an optional external Light.
	- **BedButtonInteraction.cs**: XR button logic for bed controls; while held via grip, repeatedly calls the appropriate BedController action and shows pressed color.
	- **BedController.cs**: Orchestrates table, tray, and cart movement for the bed: up/down, home, and fixed-distance positioning logic.
	- **ButtonClusterLightController.cs**: Special ButtonLightBase for the bed panel cluster (up/down/home/crosshair) mapping names to four light objects.
	- **ButtonLightBase.cs**: Abstract helper that maps button names to light GameObjects and exposes TurnButtonOn/Off with internal state tracking.
	- **ButtonLightController.cs**: ButtonLightBase for cart panel lights (up/down/park/dock/power/hands) and gating non‑power lights by the Power state.
	- **CartMovementController.cs**: Moves the cart using two XR controller handles, applying local forward motion when preconditions (power/park) are met and managing dock state.
	- **CrosshairButton.cs**: UI button that calls SpotlightController.ToggleSpotlight() when clicked.
	- **DialButton.cs**: UI button driving a DialController and BedController: on press moves bed a fixed distance and while held drives DialDown/Up.
	- **DialController.cs**: XR-grabbable dial that rotates within limits and moves the bed up/down based on rotation, easing back to a home angle when released.
	- **DisableButtons.cs**: Enables/disables a set of real buttons and alternate “dummy” buttons based on a trigger GameObject’s active state.
	- **SpotlightController.cs**: Toggles a Light component and reports via CheckerInterface whether the spotlight has ever been turned on (“Landmarking” step).
	- **VRHoldButton.cs**: Base IPointerDown/Up “hold” button that continuously runs virtual ifHeld/ifNotHeld and manages a light entry via ButtonLightBase.
- **MainMenuScripts/**
	- **DataBanker.cs**: Simple singleton storing selected exam type, gender, and whether the first check has run, with getters/setters.
	- **MainMenuFunc.cs**: Handles gender/exam selection buttons, canvas swapping, exam storage in DataBanker, XR rig teleport, and hint refresh.
- **PatientInteraction/**
	- **DialogueController.cs**: Drives user/patient speech panels, timed fades, and dialogue sequences, including initial prompts and selection dialogue.
	- **IPatientStateCondition.cs**: Interface for objects that can block patient state changes by reporting if a change is allowed and a refusal message.
	- **PatientMenu.cs**: Builds the radial/vertical patient menu from configured items, instantiates PatientMenuButtons, and shows/hides it per state.
	- **PatientMenuButton.cs**: Individual patient menu button that animates in/out and, when clicked, triggers dialogue plus the menu item’s callback.
	- **PatientMover.cs**: Moves the patient along BezierSpline paths with roughly constant speed and calls back to advance patient state when done.
	- **PatientStateChangeCollider.cs**: Trigger zone that, when an expected object comes near and state is appropriate, requests a PatientStateManager transition.
	- **PatientStateManager.cs**: Central FSM for patient pose/flow; runs conditions, animations, movements, transforms, and configures menu items per state.
	- **ShowSpeechButtonZone.cs**: Shows or hides the speech-button UI based on XR ray directions relative to camera pitch, with a small slide animation.
- **SnapPoint/**
	- **BedSheetStatus.cs**: Checker/Returned implementation that validates bed sheet on bed, and later its disposal into a laundry hamper container.
	- **BottomCoilStatus.cs**: Checks if a bottom coil is attached under its hierarchy (by Coil tag) for error checking in head exams.
	- **CallBell.cs**: ISnappable call bell that tracks in‑hand vs returned state and exposes both setup and teardown checks/labels.
	- **CoilContainerFlipper.cs** Collects non‑head coil snap points and flips them 180° around the container based on exam type, skipping occupied points.
	- **CoilPatientAnimationTrigger.cs**: On coil snap, waits until patient reaches a starting state, then advances them to a target state.
	- **CoilStatus.cs**: For coil selection/return: verifies correct coil parts for the current exam and that all coils are back on the shelf container.
	- **EarplugStatus.cs**: Tracks earplugs worn vs discarded; correct when worn, returned when removed and placed in a garbage Container.
	- **GhostCollider.cs**: Utility that continually re‑enables its Collider component each frame so “ghost” colliders stay active.
	- **HeadCoilSnapCondition.cs**: ISnapCondition that restricts top head coil placement until patient is in the “lyingHeadToScanner” state for head exams.
	- **HeadSnapPointEnabler.cs**: Enables/disables head snap points based on whether a removable headpiece is still attached to a reference snap point.
	- **KneeCoilSnapCondition.cs**: ISnapCondition for knee coils: bottom can always snap; top only when exam is Knee and patient state is “kneeLowered”.
	- **MeshSwitcher.cs**: On trigger with a target object, hides itself and enables the target’s MeshRenderer to swap visible meshes.
- **SnapPoint/**
	- **ModelSwitcher.cs**: Toggles between a “starting” and “alternate” model (and their colliders) either via trigger with a target object or explicit SwitchLogic, e.g., folded vs expanded asset.
	- **PillowStatus.cs**: Checker/Returned script that treats a pillow as placed if it has any children attached, and returned when removed; also exposes a Flip() helper to reposition it.
	- **SnapGhost.cs**: Ghost mesh preview at a snap point that enables its MeshRenderer when a coil with matching mesh enters and SnapPoint conditions allow snapping.
	- **SnapGrabInteractable.cs**: Custom XRGrabInteractable that only allows selection within a grab radius, tracks controller offset, preserves rotation with optional frozen axes, and raises grab/release events.
	- **SnapPoint.cs**: Core snap system: snaps coils (or a specific expected object) to a configured local pose, checks optional ISnapCondition, disables physics, sets interaction layer, and notifies ISnappable; also turns off ghost visuals after snapping.
- **SpeechBubble/**
	- **Billboard.cs**: Rotates its GameObject each LateUpdate so it always faces the main camera.
	- **PatientClickInteractable.cs**: XRBaseInteractable that, while hovered, listens for left/right activate InputActions and invokes a UnityEvent when pressed.
	- **PatientHoverInteractable.cs**: XRBaseInteractable that shows a speech bubble CanvasGroup on hover and fades it out after a delay when no longer hovered.
	- **SpeechBubbleBuilder.cs**: Programmatically builds a world‑space speech bubble with TextMeshPro and button, links it to PatientMenu and hover handling.
	- **SpeechBubbleHoverHandler.cs**: UI hover bridge that forwards pointer enter/exit to a PatientHoverInteractable’s logic.
	- **SpeechBubbleTriggerHelper.cs**: Fallback trigger that finds PatientMenu and shows it when a keyboard/controller InputAction fires.
- **Splines/**
	- **Bezier.cs**: Static helper providing 2‑ and 3‑point Bezier position and first‑derivative calculations for splines.
	- **BezierControlPointMode.cs**: Enum backing BezierSpline’s control‑point modes (e.g., Free/Aligned/Mirrored) from the Catlike Coding spline tutorial.
	- **BezierSpline.cs**: Stores spline control points and modes, evaluates world‑space positions/directions, and supports adding new Bezier curve segments.
	- **SplineWalker.cs**: Moves a Transform along a BezierSpline at roughly constant world speed, auto‑adjusting step size and orientation.
	- **Editor/**
		- **BezierSplineInspector.cs**: Custom Scene/Inspector editor to visualize and edit BezierSpline control points, modes, and add new curves.
- **Tissue/**
	- **BedSmudger.cs**: After a short delay, checks a ReturnedInterface on the same object and, if “returned”, applies a smudge to the patient bed via Tissue.ApplySmudge().
	- **ReturnToParent.cs**: Records an object’s original local parent/transform and can snap it back via ReturnToParentObject().
	- **Tissue.cs**: Implements ReturnedInterface for cleaning: tracks dirty objects, applies/removes “smudge” children, and reports whether all smudges are cleared.
- **Touchscreen/**
	- **ClickableCanvas.cs**: IPointerClickHandler that expands/collapses a UI panel with a scale animation and toggles other UI groups on open/close.
	- **ComfortController.cs**: Drives room comfort UI: brightness slider controls Light intensities and label; fan slider controls audio pitch/volume and label.
	- **TouchscreenMenuScript.cs**: Switches between Comfort/Physio/Settings panels and toggles the patient + side panels for the touchscreen menu.
- **VRBlink/**
	- **VRBlink.cs**: UI eyelid animation that plays a full-screen blink (moving/fading two RectTransform eyelids) on demand.- 

---

## Other Directories  

1. **Library**  
   - Unity-generated folder containing internal libraries and references for the project.

2. **Logs**  
   - Stores logs related to the Unity build process or runtime errors.

3. **Packages**  
   - Unity packages used in the project, including third-party and custom packages.

4. **ProjectSettings**  
   - Unity project settings, including configurations for the Unity Editor, build targets, and plugins.

5. **Temp**  
   - Unity temporary files created during the development process (can be cleared).

6. **UserSettings**  
   - Contains user-specific settings for Unity.

7. **Builds**
   - Where Unity builds the last `.apk` of the project.

---