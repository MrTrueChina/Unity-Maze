﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
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
        Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 moveDirection = _transform.TransformDirection(direction); // Transform.TransformDirection(Vector3 direction)：获取以Transform方向为基准，向参数方向移动需要的世界空间方向
        _transform.Translate (moveDirection * _speed * Time.deltaTime,Space.World);
    }

    private void OnDrawGizmos()
    {
        Debug.Log(Vector3.forward);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(_transform.position, _transform.position + _transform.TransformDirection(Vector3.forward));
    }
}
