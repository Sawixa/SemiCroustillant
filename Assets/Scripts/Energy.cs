using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayerMovements))]
public class Energy : MonoBehaviour
{

    [SerializeField, Range(0,100)] private float _energyLevel;

    [Tooltip("Regain d'énergie")]
    [SerializeField] private float _energyRegen;

    [SerializeField] private AnimationCurve _energyCoost;

    private bool dying;

    //Internal
    private PlayerMovements _playerMovements;

    private void Awake()
    {
        _playerMovements = GetComponent<PlayerMovements>();
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
            _energyLevel = Mathf.Clamp(_energyLevel + (_energyRegen - _energyCoost.Evaluate(_playerMovements.speed.magnitude / _playerMovements.maxSpeeds[_playerMovements.maxSpeeds.Length - 1])) * Time.deltaTime, 0f, 100f);
            Debug.Assert(_playerMovements.maxSpeeds[_playerMovements.maxSpeeds.Length] > float.Epsilon, "Vitesse maximale nulle.");
        }

        if (_energyLevel < float.Epsilon && !dying)
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        Array.Clear(_playerMovements.maxSpeeds, 0, _playerMovements.maxSpeeds.Length);
        dying = true;
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
