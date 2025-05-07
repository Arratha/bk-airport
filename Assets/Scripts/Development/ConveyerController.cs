using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Airport.Development
{
    public class ConveyerController : MonoBehaviour
    {
        [SerializeField] private float min;
        [SerializeField] private float max;
        [SerializeField] private float speed;
        
        [Space, SerializeField] private Collider left;
        [SerializeField] private Collider right;

        private int _direction;
        private float _startingPos;
        
        private Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = Camera.main;
            _startingPos = transform.position.z;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
                
                if (!Physics.Raycast(ray, out var hit))
                {
                    return;
                }

                if (hit.collider == left)
                {
                    _direction = -1;
                }

                if (hit.collider == right)
                {
                    _direction = 1;
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                _direction = 0;
            }
        }

        private void FixedUpdate()
        {
            var z = Mathf.Clamp(transform.position.z + _direction * speed, _startingPos + min,
                _startingPos + max);

            transform.position = new Vector3(transform.position.x, transform.position.y, z);
        }
    }
}