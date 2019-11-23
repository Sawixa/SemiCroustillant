﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovements : MonoBehaviour
{
    [HideInInspector] public Vector2 speed;

    public Vector2 Position
    {
        get
        {
            return transform.position;
        }

        set
        {
            transform.position = new Vector3(value.x, value.y, transform.position.z);
        }
    }

    private Rigidbody2D _rigidBody;

    [Tooltip("0:lent\n1:base\n2:rapide")]
    public float[] maxSpeeds = new float[3];
    private int _gear; // 0 1 or 2

    [SerializeField] private float _dashMultiplier;
     void Start()
    {
        _rigidBody = transform.GetComponent<Rigidbody2D>();
        _gear = 1;
        Debug.Assert(_rigidBody != null);
        Debug.Assert(maxSpeeds[0] < maxSpeeds[1] && maxSpeeds[1] < maxSpeeds[2]);
        Debug.Assert(_dashMultiplier > 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(Vector2 delta)
    {
        //Vector2 newpos = Position + delta * _maxSpeeds[_gear];

        speed = delta *maxSpeeds[_gear] ;//for Dash()
        _rigidBody.velocity = speed;        
    }

    public void Dash()
    {
        Debug.Log("Dash");
        _rigidBody.velocity = speed * _dashMultiplier;
    }

    /*
     * Upgrade speed
     */
    public void UpGear(float delta_time)
    {
        if (_gear >= 2)
            _gear = 2;
        else
            ++_gear;
    }

    /*
     * downgrade speed
     */
    public void DownGear(float delta_time)
    {
        if (_gear <= 0)
            _gear = 0;
        else
            --_gear;
    }
}
