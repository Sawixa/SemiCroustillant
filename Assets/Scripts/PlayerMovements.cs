using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovements : MonoBehaviour
{
    private TrailRenderer _trailRenderer;
    [SerializeField] private ParticleSystem _dashParticle;
    [SerializeField] private ParticleSystem _dashParticle2;
    [SerializeField] private float _particleOffset = .5f;

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

    //Durée totale du knockback
    private float _knockBackTotalDuration;
    //Durée restante de knockback
    private float _knockBackDuration;
    //Direction du knockback
    private Vector2 _knockBackDirection;

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


    private void Awake()
    {
        _trailRenderer = GetComponent<TrailRenderer>();
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
        IsDashing = false;
        _knockBackDirection = Vector2.zero;
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

        //TODO Check l'exponentiel ici
        _knockBackDirection = (_knockBackDuration > _knockBackTotalDuration) ? Vector2.zero : Mathf.Lerp(_knockBackDirection.magnitude,0f,_knockBackDuration/_knockBackTotalDuration)*_knockBackDirection.normalized;
        _knockBackDuration += Time.deltaTime;

        float angle = Mathf.Atan2(Speed.y,Speed.x)*180/Mathf.PI;
        _dashParticle.transform.rotation = Quaternion.Euler(angle, -90, 0);
        _dashParticle2.transform.rotation = Quaternion.Euler(angle, -90, 0);
        _dashParticle2.transform.position = transform.position + (Speed.magnitude == 0 ? new Vector3(_particleOffset, 0, 0) :(Vector3)Speed.normalized) * _particleOffset;
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
        if (!IsDashing && _knockBackDuration < _knockBackTotalDuration)
        {
            _speed += _knockBackDirection;
            Debug.Log("KnockBack!");
        }

        _rigidBody.velocity = _speed;
    }

    public void Dash()
    {
        StartCoroutine(DashCoroutine());
    }

    IEnumerator DashCoroutine()
    {
        IsDashing = true;
        _trailRenderer.enabled = true;
        _dashParticle.gameObject.SetActive(true);
        _dashParticle2.gameObject.SetActive(true);
        yield return new WaitForSeconds(_dashLength);
        IsDashing = false;
        yield return new WaitForSeconds(_trailRenderer.time);
        _trailRenderer.enabled = false;
        _dashParticle.Stop();
        _dashParticle2.Stop();
        yield return new WaitForSeconds(_dashParticle.main.startLifetime.constant);
        _dashParticle.gameObject.SetActive(false);
        _dashParticle2.gameObject.SetActive(false);
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

    public void KnockBack(Vector2 direction, float intensity, float duration)
    {
        Debug.Assert(duration > float.Epsilon);

        _knockBackDirection = direction.normalized * intensity;
        _knockBackDuration = 0f;
        _knockBackTotalDuration = duration;
    }
}
