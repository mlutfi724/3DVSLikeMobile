using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePickupObjects : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;

    private void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    }

    private void Update()
    {
        //Make sure you are using the right parameters here
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, 0);
    }
}