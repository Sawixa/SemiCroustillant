using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class EnnemyIA : MonoBehaviour
{
    protected Light2D _playerLight;
    

    protected Vector2 Position
    {
        get
        {
            return transform.position;
        }
        set
        {
            _rigidBody.MovePosition(value);
        }
    }

    private bool _isSpotted = false;

    [SerializeField] protected float _energyDamage;
    public float EnergyDamage
    {
        get
        {
            return _energyDamage;
        }
        private set
        {
            _energyDamage = value;
        }
    }

    /*[SerializeField] protected float _knockBack;
    public float KnockBack
    {
        get
        {
            return _knockBack;
        }
        private set
        {
            _knockBack = value;
        }
    }*/

    [SerializeField] protected float _step;
    [SerializeField] private AnimationCurve _spottedSpeed;
    private float _speed;
    private float _timeSinceSpotted;

    protected Vector2 _dir;

    //Internal
    private Rigidbody2D _rigidBody;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _timeSinceSpotted = 0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        _playerLight = FindObjectOfType<PlayerMovements>().GetComponentInChildren<Light2D>();
        _dir = Vector2.left;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(Position, _playerLight.transform.position);
        if (distance < _playerLight.pointLightOuterRadius - float.Epsilon)
            _isSpotted = true;
        if (Position.x < _playerLight.transform.position.x - 1)
            _isSpotted = false;
    }

    private void FixedUpdate()
    {
        _speed = _spottedSpeed.Evaluate(_timeSinceSpotted);

        if (_isSpotted)
        {
            Turn();

            _timeSinceSpotted += Time.deltaTime;
        }
        _rigidBody.angularVelocity = 0f;
        Position += _dir * _speed * Time.deltaTime;
    }

    protected virtual void Turn()
    {
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        _isSpotted = false;
        Destroy(gameObject);
    }
}
