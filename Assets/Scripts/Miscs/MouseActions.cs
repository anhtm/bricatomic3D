using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseActions : MonoBehaviour
{
    [Header("Drag")]
    Vector3 dragOrigin;
    Vector3 oldPos;
    public float DragSpeed = 15.0f;

    [Header("Rotate")]
    Vector3 _cameraOffset;
    [Range(0.01f, 1.0f)]
    public float SmoothFactor = 0.5f;
    public float RotationsSpeed = 5.0f;
    public GameObject Target;

    [Header("Zoom")]
    float minFov = 10;
    float maxFoV = 100;
    [Range(0, 50)]
    public float zoomSensitivity = 10.0f;

    void Update()
    {
        DragCamera();
        Zoom();
    }

    private void LateUpdate()
    {
        RotateCamera();
    }

    void DragCamera()
    {

        if (Input.GetMouseButtonDown(2))
        {
            oldPos = transform.position;
            dragOrigin = Camera.main.ScreenToViewportPoint(Input.mousePosition);//Get the ScreenVector the mouse clicked
        }

        if (!Input.GetMouseButton(2)) { return; }

        Vector3 newPos = Camera.main.ScreenToViewportPoint(Input.mousePosition) - dragOrigin;    //Get the difference between where the mouse clicked and where it moved
        transform.position = oldPos - newPos * DragSpeed; //Move the position of the camera to simulate a drag, speed * 10 for screen to worldspace conversion
    }

    void RotateCamera()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _cameraOffset = transform.position - Target.transform.position;
        }

        if (!Input.GetMouseButton(1)) { return; }

        Quaternion camTurnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * RotationsSpeed, Vector3.up);
        _cameraOffset = camTurnAngle * _cameraOffset;
        Vector3 newPos = Target.transform.position + _cameraOffset;
        transform.position = Vector3.Slerp(transform.position, newPos, SmoothFactor);
        transform.LookAt(Target.transform);
    }

    void Zoom()
    {
        float fov = Camera.main.fieldOfView;
        fov += Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity;
        fov = Mathf.Clamp(fov, minFov, maxFoV);
        Camera.main.fieldOfView = fov;
    }
}
