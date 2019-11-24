using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovements : MonoBehaviour
{
    private Vector2 _speed;
    
    public Vector2 Speed
    {
        get
        {
            return _speed;
        }

        private set
        {
            _speed = value;
        }
    }

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

    private Animator _animator;
    
    [Tooltip("0:lent\n1:base\n2:rapide")]
    [SerializeField] private float[] _maxSpeeds = new float[3];
    public float[] MaxSpeeds
    {
        get
        {
            return _maxSpeeds;
        }
        private set
        {
            _maxSpeeds = value;
        }
    }
    private int _gear; // 0 1 or 2

    [SerializeField] private float _dashMultiplier;
    [SerializeField] private float _dashLength;
    private bool _isDashing;

    public bool IsDashing
    {
        get { return _isDashing; }
        private set { _isDashing = value; }
    }
     void Start()
    {
        _rigidBody = transform.GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _gear = 1;
        Debug.Assert(_rigidBody != null);
        Debug.Assert(MaxSpeeds[0] < MaxSpeeds[1] && MaxSpeeds[1] < MaxSpeeds[2]);
        Debug.Assert(_dashMultiplier > 0);
        Debug.Assert(_dashLength > 0);
        _isDashing = false;
    }

    // Update is called once per frame
    void Update()
    {
        float v = _speed.magnitude;
        double ratio = 1.0/2.0;
        //Debug.Log("Speed: "+v.ToString()+" Animator: "+_animator.GetInteger("Speed").ToString());
        if( v <= _maxSpeeds[0]* (1-ratio)+_maxSpeeds[1]*ratio)
        {
            _animator.SetInteger("Speed", 0);
        } else if( v >= _maxSpeeds[2]*(1-ratio)+_maxSpeeds[1]*ratio)
        {
            _animator.SetInteger("Speed", 2);
        } else 
        {
            _animator.SetInteger("Speed", 1);
        }
    }

    public void Move(Vector2 delta)
    {
        if (delta.magnitude > 1)
        {
            delta.Normalize();
        }
        Vector2 newSpeed = delta * _maxSpeeds[_gear];       
        newSpeed *= _isDashing ? _dashMultiplier : 1;
        _speed = (_speed+newSpeed)*0.5f;//mean value
        //_speed = newSpeed;
        _rigidBody.velocity = _speed;        

    }

    public void Dash()
    {
        StartCoroutine(DashCoroutine());
    }

    IEnumerator DashCoroutine()
    {
        _isDashing = true;
        yield return new WaitForSeconds(_dashLength);
        _isDashing = false;
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
