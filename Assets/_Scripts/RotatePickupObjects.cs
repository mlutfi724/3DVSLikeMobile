using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePickupObjects : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;

    private void Update()
    {
        //Make sure you are using the right parameters here
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, 0);
    }
}