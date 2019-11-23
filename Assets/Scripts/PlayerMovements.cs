using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
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
        _rigidBody = transform.GetComponent<Rigidbody2D>();
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
        //Vector2 newpos = Position + delta * _maxSpeeds[_gear];

        _speed = delta *_maxSpeeds[_gear] ;//for Dash()
        _rigidBody.velocity = _speed;        
    }

    public void Dash()
    {
        Debug.Log("Dash");
        _rigidBody.velocity = _speed * _dashMultiplier;
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
