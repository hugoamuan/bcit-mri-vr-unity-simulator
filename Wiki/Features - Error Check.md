# MRI Workflow Error Checking Overview

**Purpose:**  
Error checking ensures that certain key tasks in the MRI workflow are performed correctly by the participating student. It also provides a visual representation of each task's status—whether it has been completed or not.

> **Note (as of May 16, 2025):**  
> The tasks are order-specific. However, not completing certain tasks does **not** restrict the user from progressing through the workflow.

---

## Scripts Handling Error Checking

### ✅ Checker Interface
- **Role:** Interface for all checkable objects.
- **Responsibilities:**
  - Returns a label for the task.
  - Returns a result indicating if the task is complete (`true`) or not (`false`).

### 📚 Base Check
- **Type:** Abstract class.
- **Role:** Extended by all scripts performing checks.
- **Responsibilities:**
  - Defines shared methods and functionality needed to carry out a check.

### 🛑 Error Check
- **Role:** Primary script handling checks.
- **Usage:** Utilized in the `StartScanManager` and `DoubleDoor` objects.
- **Function:** Manages **first and second** task checks.

### 🔄 Returned Check
- **Role:** Handles checks expected to return `false`.
- **Example Use Case:**  
  - A coil that has been placed incorrectly or returned to the wrong spot should trigger a `false` result.

---

## ✅ Error Check Task Sections

### 1. Prepping the Room
- **Bed Lowering** → _Handled by:_ `BedController`
- **Hand Sanitizing** → _Handled by:_ `SanitizeHands`
- **Bed Sheet Placement**
- **Pillow Placement** → _Handled by:_ `PillowSnapPoint`

### 2. Prepping the Patient and Starting Scan
- Patient in Correct Position
- Landmarking Correctly
- Correct Coil Placement
- Ear Plugs Inserted
- Headphones Worn
- Doors Closed

### 3. Dismissing the Patient and Room Teardown
- Bed Sheet Placed in Hamper
- Pillow Removed and Returned to Shelf
- Coil Removed and Returned to Shelf
- Earplugs and Headphones Removed
- All Equipment Sanitized using **Lysol Wipe**

---