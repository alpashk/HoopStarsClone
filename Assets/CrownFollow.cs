using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrownFollow : MonoBehaviour
{
    public Transform target;

    [SerializeField]
    private float yOffset = 2f;

    [SerializeField]
    private float rotateSpeed = 2f;

    private void FixedUpdate()
    {
        transform.position = new Vector3(target.position.x, target.position.y + yOffset, 0);
        transform.Rotate(Vector3.forward, rotateSpeed);
    }
}
