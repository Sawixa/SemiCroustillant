using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Rendering.LWRP;

[RequireComponent(typeof(PlayerMovements))]
public class Energy : MonoBehaviour
{

    private float _energyLevel;

    public float energy => this._energyLevel;

    [Tooltip("Regain d'énergie")]
    [SerializeField] private float _energyRegen;

    [SerializeField] private AnimationCurve _energyCost;

    [SerializeField] private AnimationCurve _lightRadius;

    [SerializeField] private LayerMask _ennemyLayer;

    [SerializeField] private float _damageRecoveryTime;

    private float _timeSinceDamaged;

    private bool dying;

    //Internal
    private PlayerMovements _playerMovements;
    private Light2D _light2D;
    private Rigidbody2D _rigidBody;

    private void Awake()
    {
        _playerMovements = GetComponent<PlayerMovements>();
        _light2D = GetComponentInChildren<Light2D>();
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _energyLevel = 100f;
    }

    // Update is called once per frame
    void Update()
    {

        //clamp new energy level between 0 and 100
        if (!dying)
        {
            Debug.Assert(_playerMovements.MaxSpeeds[_playerMovements.MaxSpeeds.Length - 1] > float.Epsilon, "Vitesse maximale nulle.");
            if(!_playerMovements.IsDashing)
                _energyLevel = Mathf.Clamp(_energyLevel + (_energyRegen - _energyCost.Evaluate(_playerMovements.Speed.magnitude / _playerMovements.MaxSpeeds[_playerMovements.MaxSpeeds.Length - 1])) * Time.deltaTime, 0f, 100f);
            _light2D.pointLightOuterRadius = _lightRadius.Evaluate(_energyLevel / 100f);
        }

        if (_energyLevel < float.Epsilon && !dying)
        {
            //StartCoroutine(Die());
        }

        _timeSinceDamaged += Time.deltaTime;

    }

    public void add(float x)
    {
        _energyLevel += x;
    }

    private IEnumerator Die()
    {
        Array.Clear(_playerMovements.MaxSpeeds, 0, _playerMovements.MaxSpeeds.Length);
        dying = true;
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == Mathf.RoundToInt(Mathf.Log(_ennemyLayer.value, 2)) && _timeSinceDamaged > _damageRecoveryTime + float.Epsilon)
        {
            _timeSinceDamaged = 0f;
            EnnemyIA ennemy = collision.gameObject.GetComponent<EnnemyIA>();
            Debug.Assert(ennemy != null);
            _energyLevel = Mathf.Clamp(_energyLevel - ennemy.EnergyDamage, 0, 100);
            //_rigidBody.AddForce((Vector2)(collision.transform.position - transform.position)*ennemy.KnockBack,ForceMode2D.Impulse);
        }
    }
}
