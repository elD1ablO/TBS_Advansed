using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Test : MonoBehaviour
{
    [SerializeField] Unit unit;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            GridSystemVisual.Instance.HideAllGridPositions();
            GridSystemVisual.Instance.ShowGidPositionList(unit.GetMoveAction().GetValidGridPositionList());
        }
    }


}
