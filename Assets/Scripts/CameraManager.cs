using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] GameObject actionCamera;    

    private void Start()
    {        
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
        BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;
    }
    void ShowActionCamera()
    {
        actionCamera.SetActive(true);
    }
    void HideActionCamera()
    {
        actionCamera.SetActive(false);
    }

    void BaseAction_OnAnyActionStarted(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:
                Unit shooterUnit = shootAction.GetUnit();
                Unit targetUnit = shootAction.GetTargetUnit();

                Vector3 cameraCharacterHeigt = Vector3.up * 1.7f;
                Vector3 shootDitection = (targetUnit.GetWorldPosition() - shooterUnit.GetWorldPosition()).normalized;

                float shoulderOffsetAmount = 0.5f;
                Vector3 shoulderOffset = Quaternion.Euler(0, 90, 0) * shootDitection * shoulderOffsetAmount;

                Vector3 actionCameraPosition = shooterUnit.GetWorldPosition() + cameraCharacterHeigt + shoulderOffset + (shootDitection * -1);

                actionCamera.transform.position = actionCameraPosition;
                actionCamera.transform.LookAt(targetUnit.GetWorldPosition() + cameraCharacterHeigt);

                ShowActionCamera();
                break;

        }
    }

    void BaseAction_OnAnyActionCompleted(object sender, EventArgs e)
    {
        switch (sender)
        {
            case ShootAction shootAction:
                HideActionCamera();
                break;
        }
    }
}
