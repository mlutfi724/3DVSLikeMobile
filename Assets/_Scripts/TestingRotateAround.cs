using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingRotateAround : MonoBehaviour
{
    //[SerializeField] private Transform _rotatePoint;
    //[SerializeField] private float _rotateSpeed;

    //private void Update()
    //{
    //    transform.RotateAround(_rotatePoint.position, Vector3.up, _rotateSpeed * Time.deltaTime);
    //}

    public Transform centerObject; // The object to orbit around
    public float rotationSpeed = 20f; // Speed of rotation

    private void Update()
    {
        // Ensure there is a center object assigned
        if (centerObject == null)
        {
            Debug.LogWarning("Please assign a center object in the inspector.");
            return;
        }

        // Calculate the desired position in a circular orbit
        Vector3 orbitPosition = CalculateOrbitPosition();

        // Set the object's position to the calculated orbit position
        transform.position = orbitPosition;

        // Rotate the object around the center object
        RotateAroundCenter();
    }

    private Vector3 CalculateOrbitPosition()
    {
        // Calculate the desired position in a circular orbit
        float angle = Time.time * rotationSpeed;
        float x = centerObject.position.x + Mathf.Cos(angle) * 2f; // You can adjust the radius of the orbit
        float z = centerObject.position.z + Mathf.Sin(angle) * 2f; // You can adjust the radius of the orbit
        float y = centerObject.position.y;

        return new Vector3(x, y + 0.5f, z);
    }

    private void RotateAroundCenter()
    {
        // Rotate the object around the center object
        transform.RotateAround(centerObject.position, Vector3.up, rotationSpeed * Time.deltaTime);
    }
}