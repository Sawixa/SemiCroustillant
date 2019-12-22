using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class EnnemyIA : MonoBehaviour
{
    protected Light2D _playerLight;
    public Light2D PlayerLight
    {
        get
        {
            return _playerLight;
        }
        set
        {
            _playerLight = value;
        }
    }

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

    [SerializeField] private AnimationCurve _knockBackCurve;
    public AnimationCurve KnockBackCurve
    {
        get
        {
            return _knockBackCurve;
        }
    }

    [SerializeField] private AnimationCurve _selfKnockBackCurve;
    private float _timeSinceKnockBack;
    private Vector2 _knockBack;
    private bool _isKnockedBack = false;

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

    private void OnEnable()
    {
        PlayerMovements player = FindObjectOfType<PlayerMovements>();
        if (player != null && _playerLight == null)
            _playerLight = player.GetComponentInChildren<Light2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerMovements player = FindObjectOfType<PlayerMovements>();
        if (player != null && _playerLight == null)
            _playerLight = FindObjectOfType<PlayerMovements>().GetComponentInChildren<Light2D>();
        _dir = Vector2.left;
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerLight != null)
        {
            float distance = Vector2.Distance(Position, _playerLight.transform.position);
            if (distance < _playerLight.pointLightOuterRadius - float.Epsilon)
                _isSpotted = true;
            if (Position.x < _playerLight.transform.position.x - 1)
                _isSpotted = false;
        }

        _timeSinceKnockBack += Time.deltaTime;
        
    }

    private void FixedUpdate()
    {
        _speed = _spottedSpeed.Evaluate(_timeSinceSpotted);

        if (_isSpotted)
        {
            Turn();

            _timeSinceSpotted += Time.fixedDeltaTime;
        }
        _rigidBody.angularVelocity = 0f;
        Vector2 displacement = (_dir * _speed) * Time.fixedDeltaTime;
        Vector2 knockBack = Vector2.zero;

        //Si on est en knockback on évalue la vitesse de poussée et on l'ajoute (si elle est nulle alors on est plus knockback)
        if (_isKnockedBack)
        {
            knockBack = ManageKnockBack();
        }

        Position += displacement + knockBack;
    }

    private Vector2 ManageKnockBack()
    {
        float magn = _knockBackCurve.Evaluate(_timeSinceKnockBack);
        if (magn < float.Epsilon)
        {
            _isKnockedBack = false;
            return Vector2.zero;
        }
        else
        {
            return magn * _knockBack * Time.fixedDeltaTime;
        }
    }

    protected virtual void Turn()
    {
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    public void KnockBack(Vector2 direction)
    {
        _timeSinceKnockBack = 0f;
        _isKnockedBack = true;
        _knockBack = direction;
    }

    private void OnBecameInvisible()
    {
        _isSpotted = false;
        //Destroy(gameObject);
    }
}
