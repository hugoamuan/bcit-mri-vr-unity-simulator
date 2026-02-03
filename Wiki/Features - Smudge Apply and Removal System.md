# Making a GameObject Smudgable

This document outlines the steps required to make a GameObject smudgable using Unity's particle system, colliders, and event-driven scripts.

---

## GameObjects Involved

- `Tissue`: Holds the Tissue.cs script
- `LCD` and `ButtonCluster`: GameObjects that are in the Dirty Objects array in the Tissue.cs script (assigned in Inspector)
- `Patient`: Holds PatientStateManager.cs script that controls when the smudge gets applied to all objects
- `Smudge`: Child object under smudgable objects
- Smudgable parents such as:
  - `Pillow_Head`
  - `Body`
  - `Coil_TxRxKnee18_Top`

---

## Scripts involved 

- Tissue.cs: Attached to the `Tissue` GameObject
- GhostCollider.cs: Attached to all `Smudge` GameObjects
- PatientStateManager.cs: Controls in which animation cycle smudge gets applied. 
    - Click on the `Patient` GameObject and in the Inspector, open 'Patient State Manager > Patient States > walkingFromTable > Transition > Additional Events ()' to see how the smudge gets triggered on all objects

---

## Steps to Configure a Smudgable Object

### 1. Create Smudge Child Object

Under the GameObject to be smudged:

```text
Right-click parent → Create Empty → Rename to "Smudge"
```

This will serve as the container for the smudge particle effect and collider.

---

### 2. Add Particle System

1. Locate an existing `Smudge` GameObject.
2. In the **Inspector**, right-click the `Particle System` component → **Copy Component**.
3. Paste it as a new component on the new `Smudge` GameObject.

---

### 3. Configure Particle System Shape

- Open the **Shape** module.
- Set the **Mesh** property:
  - If the parent has a mesh: drag it into the Shape's Mesh field.
  - If not: change the shape type to `Box`.  
    _(Example: See `LCD` or `ButtonCluster` GameObject for box-shaped smudge setup.)_

---

### 4. Add Collider

1. Add a **Mesh Collider** (or **Box/Sphere Collider** if no mesh is available).
2. Configure the collider:
   - Check `Convex`
   - Check `Is Trigger`
   - Assign the same mesh used in the particle system’s shape

---

### 5. Add `GhostCollider` Script

Attach the `GhostCollider` script to the `Smudge` GameObject.  
This ensures the collider remains active at runtime.

---

### 6. Disable the Smudge GameObject

In the **Inspector**, uncheck the Smudge GameObject to disable it by default.  
The object will be enabled during gameplay when needed.

---

### 7. Link to `Tissue` in OnGrabbed Event

1. Select the parent of the `Smudge` GameObject.
2. Locate the `SnapGrabInteractable` script.
3. Scroll to the **OnGrabbed** UnityEvent section.
4. Add a new event:
   - Drag the `Tissue` GameObject into the object field.
   - In the dropdown, select:
     ```
     Tissue → AddDirtyObject(GameObject)
     ```
   - Drag the parent GameObject (that has the Smudge as a child) into the parameter field.

---

## Example Hierarchy

```plaintext
Coil_TxRxKnee18_Top
├── MeshRenderer
├── SnapGrabInteractable
└── Smudge (disabled by default)
    ├── Particle System
    ├── Mesh Collider (Convex + Is Trigger)
    └── GhostCollider (script)
```

---

## Notes

- Always call the child object `Smudge`.
- Ensure the collider and particle system share the same mesh.
- For GameObjects without a mesh, fall back to `Box` shape and box collider.
- Refer to `LCD` or `ButtonCluster` GameObject for a non-mesh-based setup.