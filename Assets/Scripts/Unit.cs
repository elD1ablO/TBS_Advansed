using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    Vector3 targetPosition;

    void Update()
    {
        float stoppingDistance = .1f;
        if ( Vector3.Distance(transform.position, targetPosition) > stoppingDistance);
        {
            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            float moveSpeed = 4f;
            transform.position += moveDirection * Time.deltaTime * moveSpeed;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Move(MouseWorld.GetPosition());
        }
        
    }

    void Move(Vector3 targetPosition)
    {
        this.targetPosition = targetPosition;

    }
}
