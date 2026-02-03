# Interacting with the patient

This document outlines the objects and scripts used in the various processes involved in interacting with the patient, including:

- [Talking to the patient](#markdown-header-talking-to-the-patient)
- [Patient menu](#markdown-header-patient-menu)
- [Patient state management](#markdown-header-patient-state-management)
- [Patient movement](#markdown-header-patient-movement)

---

## Talking to the Patient

### GameObjects Involved

- `SpeechMenu` (XR Interaction Setup > XR Origin (XR Rig) > Camera Offset > Main Camera): Holds the different speech menu objects, has the `ShowSpeechButtonZone` and `DialogueController` script components
- `SpeechButton`: the button used to open the speech menu
- `UserSpeechPanel`: the user's speech bubble
- `MenuButtons`: an empty object to which menu items are added by the PatientMenu script
- `ClickColliderHolder` (in Spine2 of Patient skeleton): Used to trigger the first lines of patient dialogue (when patient is selected)
- `KneeTarget` (in RightLowLeg1 of Patient skeleton): Used to trigger exam selection dialogue
- `HeadTarget` (in Head of Patient skeleton): Used to trigger exam selection dialogue
- `Patient`: Has PatientMenu script component responsible for creating patient menu buttons which trigger dialogue

---

### Scripts involved 

- PatientInteraction/ShowSpeechButtonZone.cs: Shows/hides the button used to open the speech menu
- PatientInteraction/DialogueController.cs: Controls the text content and animations of the user and patient speech bubbles
- PatientInteraction/PatientMenu.cs: Holds a List of PatientMenuItems, in which dialogue associated with each menu item can be defined, and creates patient menu buttons
- PatientInteraction/PatientMenuButton.cs: Attaches calls to the DialogueController to the OnClick event of each patient menu button

---

### Adding dialogue
In most cases, dialogue is initiated when the user clicks on a patient menu item. To edit the dialogue of an existing state, simply open the inspector for the Patient game object, find the menu item in the All Menu Items list in the PatientMenu component, and edit the following fields:

- Hint Text (the text that will be shown in the menu button
- Dialogue template (a string template which this and subsequent user dialogue text will follow; must include `{0}`, which will be replaced by the user dialogue text)
- User Dialogue Text (the text the will appear in the user's speech bubble)
- Patient Dialogue Text (the text the will appear in the patient's speech bubble)
- Dialogue Duration (the number of seconds for which the user speech bubble will appear)

If new dialogue is required, follow the instructions to create a new menu button, and fill in the fields listed above. 

---

## Patient Menu

### GameObjects Involved

- `Patient`: Holds the PatientMenu and PatientStateManager script components, used to define all patient menu items and states in which they appear
- `MenuButtons` (in the MainCamera of the XR Rig): An empty game object that holds all of the patient menu buttons
- `DialogueButton` (prefab): a template for the patient menu buttons

---

### Scripts involved 

- PatientInteraction/PatientStateManager.cs: Holds a list of patient states, each of which has a list of PatientMenuItem labels of menu items available in that state
- PatientInteraction/PatientMenu.cs: Manages a list of all possible patient menu items, the ones currently available in the menu, and functions for creating menu buttons and showing and hiding the menu
- PatientInteraction/PatientMenuButton.cs: Manages the data for a single patient menu button, including functions to create the button and add it to the patient menu game object, and animate its movement

---

### Creating a Patient Menu Button

In the Patient Menu component of the Patient game object, add an item to the All Menu Items list, and fill in the fields:

- Label (a name for this menu item)
- Target State Label (the label of the state the patient will change into when this button is clicked)
- Hint Text (the text that will appear on the button)
- Exam Filter (the button will only appear when the text in this field is empty or matches the name of the currently selected exam)
- Dialogue options (see Talking to the Patient section)

In order to see the patient menu button, it must be added to the menu associated with a specific patient state. In the patient states list of the PatientStateManager component of the Patient, find the state to which you would like to add the button, add an element to its Menu Items list, and enter the label (name) of your patient menu item.

---

### Notes

- The icon options in the PatientMenu script are currently unused.

---

## Patient State Management

### GameObjects Involved

- `Patient`: Holds the PatientStateManager, PatientMenu, and PatientMover script components, used to define all patient states, patient menu items, and patient movements
- `chr_m35a`: The patient model, which contains the animator used to animate the patient
- `ClickColliderHolder` (in Spine2 of the Patient skeleton): Used to change the state of the patient on patient selection
- `KneeTarget` (in RightLowLeg1 of Patient skeleton): Used to trigger patient state change once exam type is selected
- `HeadTarget` (in Head of Patient skeleton): Used to trigger patient state change once exam type is selected
- `DoubleDoor` and `UpDownBed(Bed01)`: Implement the IPatientStateCondition interface, used to check if the patient can change states
- `KneeUpStateTrigger`: Used to change patient to the knee raised stated once knee coil is brought close
- `Coil_TxRxKnee18_Base`: Changes the patient to the knee lowered state once coil is placed on the table

---

### Scripts involved 

- PatientInteraction/PatientStateManager.cs: Holds a list of patient states, and functions to handle all aspects of changing patient between states
- PatientInteraction/PatientMenu.cs: Manages a list of all possible patient menu items, and functions to use them
- PatientInteraction/PatientMover.cs: Holds a List of PatientMovements and functions to use them
- PatientInteraction/PatientStateChangeCollider.cs: Changes the patient to a given state if they are in a specified state and a specified object is brought into the collider
- PatientInteraction/IPatientStateCondition.cs: An interface with functions to be used to check whether a patient can move into a state, and what message should be returned if the check fails

---

### Creating a Patient State

In the PatientStateManager script component of the Patient, add an item to the PatientStates list, and fill out the fields:

- Label (the name of the state)
- Menu Items (a list of PatientMenuItems which will be available from the patient menu when the patient is in this state)
- Condition Objects (a list of GameObjects which implement the IPatientStateCondition interface to check before the patient can enter this state)
- Transition (the transition occurs as the patient enters this state)
    - Animation Name (the name of the animation in the patient animator to play when entering the state, or blank if no animation)
    - Parent (the object which the patient will become a child of if the Update Parent state option is checked)
    - Movement Label (the name of a patient movement to execute as this state starts)
    - Additional Events (a list of functions which will be invoked when the patient first enters this state)
- Options:
    - Flip Model (whether to invert/mirror the patient model after the transition into this state)
    - Flip Transition (whether to invert/mirror the patient model for only the transition into this state)
    - Update Parent (whether to update the parent of the patient object to the object in the Parent field of the Transition)
    - Update Rotation (whether to update the rotation of the patient model in this state)
    - Pivot Degrees (the number of degrees about the Y axis to which the patient should be rotated if Update Rotation is checked)
    - Update Position (whether to update the patient model's local position when entering this state)
    - X Position (the new local X position of the patient model if Update Position is checked)
    - Y Position (the new local Y position of the patient model if Update Position is checked)
    - Z Position (the new local Z position of the patient model if Update Position is checked)
    - Transition Offset Y (the amount to offset the patient model's Y position during the transition animation)
    - Immediate Next State (the label/name of a patient state to transition to after the current state, or blank for none)
    - Next State Delay (the amount of time in seconds to wait before changing the patient to its Immediate Next State)

---

### Notes

- Currently, many of the patient states are duplicate for the knee and head exams. This is due to the fact having the patient animate into the correct position for each exam (feet to scanner for knee and head to scanner for head), requires the patient to be sitting in a different place on the side of the table. This could possibly be fixed by adding an animation of the patient shifting up or down the table in some way, so that they are able to move between the different positions.

---

## Patient Movement

### GameObjects Involved

- `Patient`: Holds the PatientMover and PatientStateManager script components, which are used to define patient movements and the states in which they occur
- `chr_m35a`: The patient model, which contains the animator used to animate the patient walking
- `PatientPaths`: An empty game object which holds the various patient path objects

---

### Scripts involved 

- PatientInteraction/PatientStateManager.cs: Holds a list of patient states, each of which has a field to specify the label of a patient movement the patient will follow during that state
- PatientInteraction/PatientMover.cs: Holds a List of PatientMovements and functions for moving the patient along a given movement path
- All scripts in the Splines folder: These scripts define the Bezier curves that are used to define patient movement paths, and provide tools to work with them in the Unity editor

---

### Adding a Patient Movement
In the PatientMover component of patient object, add an item to the All Patient Movements list, and fill out the fields:

- Label (the name of the patient movement)
- Path Spline (the spline along which the patient will move)
- Speed
- Next Patient State Label (the label of the state that the patient should move into once they have finished moving along the path)

In order for the patient to perform a movement, it must be associated with a specific patient state. In the patient states list of the PatientStateManager component of the Patient, find the state to which you would like to add the movement, and enter the label (name) of your patient movement to the Movement Label field of the state's Transition object.

---

### Adding a Patient Path Spline
To create a spline for a patient path, create a new empty game object (preferably in the PatientPaths game object), and attach the Bezier Spline script component to it. Click a point on the spline to adjust its position or mode (the behaviour of each end of two joined curves in the spline). Click Add Curve to add an additional curve to the spline. Typically the ends of the spline can be free and the intermediate joints between curves should be aligned.