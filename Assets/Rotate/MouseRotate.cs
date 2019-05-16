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
        /*
         *  270是正上方
         *  0是前方
         *  45是斜向下
         *  90是正下方
         */
        Vector3 rotation = _cameraTransform.rotation.eulerAngles;

        rotation.x -= Input.GetAxis("Mouse Y") * _speed;
        rotation.x = rotation.x > 180 ? rotation.x - 360 : rotation.x; // Transform.rotation的欧拉角是没有负数的，和Inspector面板上显示的有正负是不一样的，因此从上到下的范围是[0-90]+[270-360]两个范围，在这里把270-360范围转为-90-0，方便后面Clamp
        rotation.x = Mathf.Clamp(rotation.x, -90, 90);

        _cameraTransform.rotation = Quaternion.Euler(rotation);
    }

    private void RotateTransform()
    {
        Vector3 rotation = _transform.rotation.eulerAngles;
        rotation.y += Input.GetAxis("Mouse X") * _speed;
        _transform.rotation = Quaternion.Euler(rotation);
    }
}
