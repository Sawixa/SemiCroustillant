using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

[RequireComponent(typeof(Rigidbody2D))]
public class EnnemyIA : MonoBehaviour
{
    [SerializeField] private Light2D _playerLight;


    private Vector2 Position
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

    [SerializeField] private float _step;
    [SerializeField] private AnimationCurve _spottedSpeed;
    private float _speed;
    private float _timeSinceSpotted;

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
            float angle = Vector2.SignedAngle(transform.right, (Vector2)_playerLight.transform.position - Position);
            float zDir = (Mathf.Abs(angle) > _step) ? (transform.rotation.eulerAngles.z - Mathf.Sign(angle) * _step) : (transform.rotation.eulerAngles.z - angle);
            transform.rotation = Quaternion.Euler(0, 180, zDir);

            

            _timeSinceSpotted += Time.deltaTime;
        }
        
        Position += new Vector2(transform.right.x, transform.right.y) * _speed * Time.deltaTime;
    }

    private void OnBecameInvisible()
    {
        _isSpotted = false;
        Destroy(gameObject);
    }
}
