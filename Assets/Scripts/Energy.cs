using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Rendering.LWRP;
using UnityEngine.UI;

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

    [SerializeField] private float _blinkTime;
    [SerializeField] private float _blinkIntensity;

    [SerializeField] private Image _energyBar;
    [SerializeField] private Image _logo;
    [SerializeField] private Color _blinkColor;

    [SerializeField] private bool _isDefensive = false;
    [SerializeField] private bool _isOffensive = false;

    private float _timeSinceDamaged;

    private bool dying;

    //Internal
    private PlayerMovements _playerMovements;
    private Light2D _light2D;
    private Rigidbody2D _rigidBody;
    [SerializeField] private ParticleSystem _deathParticles;

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
        StartCoroutine(BlinkWhenLow());
    }

    // Update is called once per frame
    void Update()
    {

        //clamp new energy level between 0 and 100
        if (!dying)
        {
            Debug.Assert(_playerMovements.MaxSpeeds[_playerMovements.MaxSpeeds.Length - 1] > float.Epsilon, "Vitesse maximale nulle.");

            if (!_playerMovements.IsDashing && _timeSinceDamaged > _damageRecoveryTime + float.Epsilon)
            {
                //Regain energy
                float cost = _energyCost.Evaluate(_playerMovements.Speed.magnitude / _playerMovements.MaxSpeeds[_playerMovements.MaxSpeeds.Length - 1]);
                _energyLevel += (_energyRegen - cost) * Time.deltaTime;
            }

            _light2D.pointLightOuterRadius = _lightRadius.Evaluate(_energyLevel / 100f);
            _energyBar.fillAmount = _energyLevel / 100f;
        }

        if (_energyLevel < float.Epsilon && !dying)
        {
            StartCoroutine(Die());
        }

        _timeSinceDamaged += Time.deltaTime;

        _energyLevel = Mathf.Clamp(_energyLevel,0f,100f);
    }

    public void add(float x)
    {
        _energyLevel += x;
    }

    private IEnumerator Die()
    {
        Array.Clear(_playerMovements.MaxSpeeds, 0, _playerMovements.MaxSpeeds.Length);
        dying = true;
        SpriteRenderer spRd = GetComponent<SpriteRenderer>();
        spRd.enabled = false;
        _rigidBody.simulated = false;
        _deathParticles.Play();
        AudioManager.PlaySFX("Sparkle");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == Mathf.RoundToInt(Mathf.Log(_ennemyLayer.value, 2)))
        {
            ManageEnnemyCollision(collision);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == Mathf.RoundToInt(Mathf.Log(_ennemyLayer.value, 2)))
        {
            ManageEnnemyCollision(collision);
        }
    }

    private void ManageEnnemyCollision(Collision2D collision)
    {
        EnnemyIA ennemy = collision.gameObject.GetComponent<EnnemyIA>();
        Debug.Assert(ennemy != null);

        //Tuer un rino
        RinoScript rinoScript = collision.gameObject.GetComponent<RinoScript>();
        if (rinoScript != null && _playerMovements.IsDashing && _isOffensive)
        {
            rinoScript.Die();
        }
        else
        {
            //Tuer un essaim
            SwarnScript swarnScript = collision.gameObject.GetComponent<SwarnScript>();
            if (swarnScript != null && _playerMovements.IsDashing && !_isDefensive)
            {
                swarnScript.Die();
            }
            //Perdre de l'énergie
            else if (_timeSinceDamaged > _damageRecoveryTime + float.Epsilon)
            {
                _playerMovements.KnockBack((_playerMovements.Position - (Vector2)ennemy.transform.position).normalized, ennemy.KnockBack, ennemy.KnockBackDuration);
                _timeSinceDamaged = 0f;
                _energyLevel = Mathf.Clamp(_energyLevel - ennemy.EnergyDamage * (_isDefensive ? .5f : 1f), 0, 100);
                AudioManager.PlaySFX("Coup");
                StartCoroutine(Blink());
            }
        }
    }

    private IEnumerator Blink()
    {
        float intensity = _light2D.intensity;
        while (_timeSinceDamaged < _damageRecoveryTime - float.Epsilon)
        {
            _light2D.intensity = _blinkIntensity;
            yield return new WaitForSeconds(_blinkTime);
            _light2D.intensity = intensity;
            yield return new WaitForSeconds(_blinkTime);
        }
        yield return null;
    }

    private IEnumerator BlinkWhenLow()
    {
        Color color = _logo.color;
        while (true)
        {
            if (_energyLevel < 33f)
            {
                _logo.color = _logo.color == _blinkColor ? color : _blinkColor;
                yield return new WaitForSeconds(_blinkTime*2);
            }
            else
            {
                _logo.color = color;
                yield return new WaitForSeconds(.5f);
            }
        }
        yield return null;
    }
}
