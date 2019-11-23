using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

[RequireComponent(typeof(PlayerMovements))]
public class Energy : MonoBehaviour
{

    [SerializeField, Range(0,100)] private float _energyLevel;

    [Tooltip("Regain d'énergie")]
    [SerializeField] private float _energyRegen;

    [SerializeField] private AnimationCurve _energyCost;

    [SerializeField] private AnimationCurve _lightRadius;

    //TODO Remove, get value from pM
    [SerializeField] private float _deb;

    //Internal
    private PlayerMovements _playerMovements;
    private Light2D _light2D;

    private void Awake()
    {
        _playerMovements = GetComponent<PlayerMovements>();
        _light2D = GetComponentInChildren<Light2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _energyLevel = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        _energyLevel = Mathf.Clamp(_energyLevel + (_energyRegen - _energyCost.Evaluate(_playerMovements.Speed.magnitude/_deb))*Time.deltaTime, 0f, 100f);
        _light2D.pointLightOuterRadius = _lightRadius.Evaluate(_energyLevel / 100f);
    }


}
