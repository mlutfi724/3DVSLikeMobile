using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatAndRotate : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float floatFrequency;
    [SerializeField] private float floatAmplitude;
    private Vector3 startPos;
    [SerializeField] private float yOffset;

    private void Awake()
    {
        startPos = new Vector3(transform.position.x, transform.position.y + yOffset, transform.position.z);
    }

    private void Update()
    {
        //Make sure you are using the right parameters here
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, 0);

        Vector3 tempPos = startPos;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * floatFrequency) * floatAmplitude;

        transform.position = tempPos;
    }
}