using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundTest : MonoBehaviour
{
    public GameObject RotatePoint;
    public float RotateSpeed;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        transform.RotateAround(RotatePoint.transform.position, Vector3.up, RotateSpeed * Time.deltaTime);
    }
}