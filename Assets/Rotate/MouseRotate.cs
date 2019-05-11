using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRotate : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    Transform _cameraTransform;

#pragma warning disable 0649
    [SerializeField]
    float _speed;

    Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    private void Update()
    {
        RotateCamera();
        RotateTransform();
    }

    void RotateCamera()
    {
        Vector3 rotation = _cameraTransform.rotation.eulerAngles;
        rotation.x -= Input.GetAxis("Mouse Y") * _speed;
        _cameraTransform.rotation = Quaternion.Euler(rotation);
        //_cameraTransform.Rotate(Vector3.left, (Input.GetAxis("Mouse Y")) * _speed, Space.World);
    }

    private void RotateTransform()
    {
        Vector3 rotation = _transform.rotation.eulerAngles;
        rotation.y += Input.GetAxis("Mouse X") * _speed;
        _transform.rotation = Quaternion.Euler(rotation);
        //_transform.Rotate(Vector3.up, (Input.GetAxis("Mouse X")) * _speed, Space.World);
    }
}
