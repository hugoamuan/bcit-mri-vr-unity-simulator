using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controller for the Zone 3 (Computer Room) door. Used to control the state 
/// and animations of the zone 4 door.
/// </summary>
public class Zone3Door : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = true;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;
        animator.SetBool("isOpen", isOpen);
    }
}
