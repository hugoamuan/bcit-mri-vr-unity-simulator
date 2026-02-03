# Release Notes

# Fall 2025

## About This Release
The focus of this update is on improving clarity, usability, and overall procedural accuracy within the training experience. Earlier builds focused mainly on establishing the core functionality, but this release is to strengthen the simulation with clearer guidance system, more stable interactions, and the addition of the head exam mission, expanding the overall training flow.

Key enhancements include labels for the simulation objects and contextual hint zones to support users as they learn proper MRI procedures, as well as the introduction of the head exam to expand the range of procedure scenarios. Alongside these additions, a considerable amount of work has also gone into stabilizing physics behavior, refining patient interactions, and addressing workflow inconsistencies that previously disrupted the user experience.

This update also includes automated testing to help ensure build stability as the project grows.

## New Features

### Hint System
- Added contextual highlight zones to guide placement of essential MRI items
- Mission-specific placement zones
- Adjusts dynamically with objects like the MRI bed.
- Placement steps now include visual highlighting assistance
- Related documentation added to project wiki references

### Label System
- Added toggleable in-world labels for interactable objects to support onboarding and debugging
- Key implementation files:
  - `LabelToggle.cs`
  - `ObjectLabel.cs`
- Label controls mapped to:
  - `Y` button on left VR controller
### Controller Guide
- Left Hand Controller
  - `X` key prompts dialogue options to talk to the patient
  - `Y` key toggles UI indicators to intuitively locate interactable objects
- Right Hand Controller
  - `A` key toggles a GUI instruction interface for new users to understand controls

### Head Exam Mission
- Implemented MRI Brain Scan as the second simulation mission
  - Added MRI Head Coil snap points on the MRI bed
- Added dedicated labels and hint zones for all head exam equipment
- Integrated feedback report logging for head exam flow validation

### Patient Interaction Update
- Added new input bindings to improve dialogue interaction reliability:
  - `X` button on left VR controller
- Removed requirement to aim above the patient to trigger dialogue
- Interaction hitbox validated for more consistent activation

---

## Test Automation

### Unity Automated Test Coverage
- Automated test cases added for:
  - Curtain interactions
  - Object grabbing behavior
  - Bedsheet placement on bed
  - Pillow placement on bed
  - Bedsheet floor-drop behavior
  - Common items floor-drop behaviour
    - Includes pillow, bedsheet and cow bell, in the moment
  - Bedsheet scales in sizerandomly(known bug captured by test)
  - Dialogue UI timing accuracy
  - Input binding validation for PNG overlay activation using `A`

---

## Quality of Life Improvements
- Rescaled MRI bed height panel button textures for readability
- Stabilized bedsheet and pillow physics and anchoring logic
- Scaled control panel UI textures for proper readability
- Increased contrast and visibility of MRI laser crosshair for alignment guidance

---

## Bug Fixes

### Build and Export
- Fixed Android export pipeline error caused by Gradle build failure
- Fixed project booting in 2D instead of VR mode during APK export
- Fixed broken speech button object reference in input bindings
- Fixed incorrect XR camera origin initialization
- Fixed missing or incorrect speech interaction bindings in controller mapping

### Physics and Collisions
- Fixed bedsheet mesh falling through MRI bed collider
- Fixed pillow random scaling, disappearing, or excessive bouncing
- Fixed coil plug collision clipping into MRI bed mesh
- Fixed call bell sinking into environment geometry
- Fixed MRI coils clipping or phasing through MRI scanner tunnel or outer-side scanner walls
- Fixed player transform void-fall and locomotion transition failures
- Added stable collision meshes to environment objects:
  - Walls
  - MRI scanner body
  - Tables
  - Chairs
  - Doors
  - Curtains
- Fixed bug that would cause user to fall down to the void when finishing a scan and going to the feedback room

### Snap and Attach
- Fixed coil snapping alignment logic for knee and head coils
- Added failsafe checks requiring patient state to be `laying down` before coil base/top attach sequencing
- Fixed right-hand transform handling for call bell pickup and placement

---

## Workflow Fixes
- Removed forced door-close requirement for Zone 3 for the second feedback report
- Updated coil snapping sequence to improve MRI mission usability and developer clarity
- Sink moved to the waiting room as it has not been implemented for sanitization purposes yet

---

## Known Limitations
- Sink interaction logic not implemented
- Bedsheet seamless texture looping fix not implemented
- Room interaction workflow still being tuned
- For more, check the [On-Hold Issues page](https://bitbucket.org/vie_bcit/mri-vr-room4/issues?sort=-status)