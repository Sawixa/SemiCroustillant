using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
   
    private Vector2 _speed;
    
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

    [SerializeField] private float[] _maxSpeeds = new float[3];
    private int _gear; // 0 1 or 2

    [SerializeField] private float _dashMultiplier;
     void Start()
    {
        _gear = 1;
        Debug.Assert(_rigidBody != null);
        Debug.Assert(_maxSpeeds[0] < _maxSpeeds[1] && _maxSpeeds[1] < _maxSpeeds[2]);
        Debug.Assert(_dashMultiplier > 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(Vector2 delta)
    {
        Vector2 newpos = Position + delta * _maxSpeeds[_gear];
        _speed = delta;//for Dash()
        _rigidBody.MovePosition(newpos);        
    }

    public void Dash()
    {
        Vector2 newpos = Position + _speed * _maxSpeeds[_gear] * _dashMultiplier;
        _rigidBody.MovePosition(newpos);
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
