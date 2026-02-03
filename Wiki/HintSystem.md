# Hint System

The **Hint System** provides a way to highlight zones for the user when holding an object and is composed of three main scripts:

---

### 1. `HintManager.cs`
Singleton manager that coordinates all `HintArea` objects and updates which hints should be visible.

**Responsibilities:**
- Tracks the currently held `HoldableItem`.
- Queries the current exam type from `DataBanker.cs`.
- Shows or hides `HintArea` highlights based on matching item and exam type.
- Keeps a list of all `HintArea` instances in the scene.

---

### 2. `HintArea.cs`
Represents a location where a hint can appear.

**Responsibilities:**
- Registers itself with `HintManager` on startup.
- Defines which item types and exam types it applies to.
- Displays or hides the assigned highlight object.

---

### 3. `HoldableItem.cs`
Attached to any object that can be picked up by the player.

**Responsibilities:**
- Notifies `HintManager` upon pickup or release.
- Holds a string identifier (`itemType`) for matching hint zones.

---

## Usage

### 1. HintManager
- Create a **HintManager** object in the project hierarchy and attach the `HintManager` script.
- Check if it already exists — if so, ensure the checkbox in the Inspector is enabled.

---

### 2. HoldableItem
- Attach this script as a **component** to any object that can be picked up.
- Under **Inspector → Item Type**, assign a **string identifier** (e.g., `"Blanket"`).  
  This value must match the `HintArea`’s item type later.

**Setting up XR Interactions:**

1. Under **Interactable Events** in the Inspector:
   - Add entries for **Select Entered (SelectEnterEventArgs)** and **Select Exited (SelectExitEventArgs)**.

2. For each:
   - Click the **plus (+)** icon.
   - Set to **Runtime Only**.
   - Drag the object with the `HoldableItem` script into the Object field.
   - Use the dropdown to select:
     - `HoldableItem.OnSelectEntered`
     - `HoldableItem.OnSelectExited`

---

### 3. HintArea
- Create a **new GameObject** to represent your highlight zone and attach `HintArea.cs`.
- In the **Inspector → Hint Configuration**, set:
  - **Item Type:** match the string used in Step 2 (`HoldableItem.itemType`).
  - **Exam Types:** specify the number of exams that will use this hint area, and enter each exam name (e.g., `"Head"`, `"Knee"`).
- Attach a glowing or highlighted **child GameObject** as the `highlightObject`. Make sure its checkbox is **disabled** so the highlight starts hidden.
- Under the inspector for the HintArea parent object add the highlight or glow as the (`Highlight Object`)

**Example Objects:**
- HoldableItems: Look at shelf items such as Pillow, Blanket or Knee Coils. (In the scene)

- HintArea: MRI Siemens/DetachableTable/UpDownBed(Bed01) (Under the hierarchy)

---

## Notes
- The **current exam type** is retrieved from `DataBanker.cs`.
- For hint zones that must move with the MRI bed, attach them as **children** of the moving object (see examples above).
- Each `HintArea` automatically registers with the `HintManager` on start — no manual setup required beyond Inspector configuration.




![HintSystem1.png](https://bitbucket.org/repo/XR95bKn/images/981334065-HintSystem1.png)
![HintSystem2.png](https://bitbucket.org/repo/XR95bKn/images/438122695-HintSystem2.png)
![HintSys3.jpg](https://bitbucket.org/repo/XR95bKn/images/3609410517-HintSys3.jpg)
![HintSystem4.jpg](https://bitbucket.org/repo/XR95bKn/images/611603812-HintSystem4.jpg)
![HintSystem5.jpg](https://bitbucket.org/repo/XR95bKn/images/143458304-HintSystem5.jpg)
![HintSystem6.jpg](https://bitbucket.org/repo/XR95bKn/images/123316349-HintSystem6.jpg)