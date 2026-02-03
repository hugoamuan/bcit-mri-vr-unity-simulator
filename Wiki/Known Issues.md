# Persisting Issues: MRI VR Simulation

This document lists all currently known persisting issues observed in the MRI VR Simulation project. Each issue includes reproduction steps, expected and actual behavior, and video demonstrations when applicable.

---

# Issue 43: Patient Dismissal Wire Stretching
**Priority:** Major  
**Status:** Persisting  

## Summary
When the patient is dismissed and begins walking to the change room, they continue moving even while still wearing the headphones and holding the call bell. The wires for both items remain attached and stretch infinitely.

## Demonstration
https://www.youtube.com/watch?v=yd4IiLWzlE0

## Description
- Patient enters the dismissal/walking state without unequipping equipment.  
- The headphone and call bell wires stay connected instead of detaching.  
- Movement causes the wires to stretch indefinitely.

## Steps to Reproduce
1. Equip patient with headphones and/or call bell.  
2. Trigger **Patient Dismissal**.  
3. Observe wire stretching as patient walks away.

## Expected Behavior
- Equipment should be removed or detached before walking.  
- No wire stretching should occur.

## Actual Behavior
- Patient walks away with equipment still attached.  
- Wire models stretch infinitely.

---

# Issue 41: Patient Positioning During MRI Exam
**Priority:** Major  
**Status:** Persisting  

## Summary
The patient can be instructed to sit up during the MRI exam while they are still inside the MRI chamber. This breaks procedure flow and results in unrealistic positioning.

## Demonstration
https://youtu.be/UB6yHes_GLk

## Description
- The system allows posture changes even during the scanning phase.  
- Patient transitions to a sitting posture inside the bore.  
- Causes clipping and breaks immersion.

## Steps to Reproduce
1. Begin MRI scanning with patient inside chamber.  
2. Trigger **Sit Up** instruction.  
3. Observe patient sitting up inside the bore.

## Expected Behavior
- Posture-changing actions should be disabled during scanning.

## Actual Behavior
- Patient sits up while still inside the MRI chamber.

---

# Issue 40: Pillow Placement for Head Exam
**Priority:** Major  
**Status:** Persisting  

## Summary
For the head exam, the pillow should be placed under the patient’s legs. Current logic does not support this correctly, and the required leg-positioning animation is missing.

## Demonstration
https://youtu.be/jJoPBj-iy1k

## Description
- Pillow placement does not attach to the correct body location.  
- Procedure cannot be completed as intended.

## Steps to Reproduce
1. Begin **Head Exam** setup.  
2. Attempt to place pillow under patient’s legs.  
3. Observe incorrect behavior.

## Expected Behavior
- Pillow should attach where legs should lie on the table.

## Actual Behavior
- Pillow does not attach where the patient's legs lie on the table.

---

# Issue 42: Lysol Wipes Disappearing When Dropped
**Priority:** Minor  
**Status:** Persisting  

## Summary
The Lysol wipes object disappears immediately upon being dropped.

## Demonstration
https://youtu.be/lnN5TQUyxn8

## Description
- The item vanishes when released instead of falling to the floor.  

## Steps to Reproduce
1. Pick up the Lysol wipes.  
2. Drop the item.  
3. Observe disappearance.

## Expected Behavior
- Item should remain in the scene and fall normally.

## Actual Behavior
- Item disappears instantly.

---

# Issue 8: Physics: Grabbable Objects
**Priority:** Minor  
**Status:** Persisting  

## Summary
Initial report listed issues with teleportation, resizing, and clipping for grabbable objects. After retesting, teleportation and resizing could not be reproduced. Clipping still occurs but only with the **Coil_CP-SpineArray_Base** object interacting with the MRI bed table.

## Demonstration
https://youtu.be/sSV26ed_lv4

## Description
- Teleporting: Not reproducible  
- Resizing: Not reproducible  
- Clipping: Confirmed, but limited to Coil_CP-SpineArray_Base  
- The coil clips into the MRI table, indicating collider or rigidbody issues.

## Steps to Reproduce
1. Grab **Coil_CP-SpineArray_Base**.  
2. Move it into contact with the MRI bed table.  
3. Observe clipping behavior.

## Expected Behavior
- Object should collide properly with the table.

## Actual Behavior
- Object clips through the table surface.  
- Other objects do not show this behavior.