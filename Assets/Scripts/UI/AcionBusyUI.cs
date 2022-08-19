using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcionBusyUI : MonoBehaviour
{
    void Start()
    {
        UnitActionSystem.Instance.OnBusyChange += UnitActionSystem_OnBusyChange;
        Hide();
    }
    void Show()
    {
        gameObject.SetActive(true);
    }
    void Hide()
    {
        gameObject.SetActive(false);
    }

    void UnitActionSystem_OnBusyChange(object sender, bool isBusy)
    {
        if (isBusy)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
}
