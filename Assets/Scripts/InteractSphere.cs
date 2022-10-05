using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSphere : MonoBehaviour, IInteractable
{
    [SerializeField] Material greenMat;
    [SerializeField] Material redMat;
    [SerializeField] MeshRenderer meshRenderer;

    GridPosition gridPosition;

    Action onInteractionComplete;
    bool isActive;
    float timer;

    bool isGreen;
    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);
        SetColorGreen(); 
        
    }

    void Update()
    {
        if (!isActive) { return; }

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            isActive = false;
            onInteractionComplete();
        }
    }
    void SetColorGreen()
    {
        isGreen = true;
        meshRenderer.material = greenMat;
    }
    void SetColorRed()
    {
        isGreen= false;
        meshRenderer.material = redMat;
    }

    public void Interact(Action onInteractionComplete)
    {
        this.onInteractionComplete = onInteractionComplete;
        isActive = true;
        timer = 0.5f;

        if (isGreen)
        {
            SetColorRed();
        }
        else
        {
            SetColorGreen();
        }
    }
}
