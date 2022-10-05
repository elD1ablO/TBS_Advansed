using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    GridPosition gridPosition;
    [SerializeField] bool isOpen;
    Animator animator;
    Action onInteractionComplete;
    bool isActive;
    float timer;
    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);
        if (isOpen)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }
    void Update()
    {
        if (!isActive) { return; }

        timer -= Time.deltaTime;

        if(timer <= 0)
        {
            isActive = false;
            onInteractionComplete();
        }
    }

    public void Interact(Action onInteractionComplete)
    {
        this.onInteractionComplete = onInteractionComplete;
        isActive = true;
        timer = 0.5f;
        if (isOpen)
        {
            CloseDoor();            
        }
        else
        {
            OpenDoor();
        }
    }

    void OpenDoor()
    {
        isOpen = true;
        animator.SetBool("isOpen", isOpen);
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, true);
    }
    void CloseDoor()
    {
        isOpen = false;
        animator.SetBool("isOpen", isOpen);
        Pathfinding.Instance.SetIsWalkableGridPosition(gridPosition, false);
    }
}
