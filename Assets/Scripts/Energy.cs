using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovements))]
public class Energy : MonoBehaviour
{

    [SerializeField, Range(0,100)] private float _energyLevel;

    [Tooltip("Regain d'énergie")]
    [SerializeField] private float _energyRegen;

    [SerializeField] private AnimationCurve _energyCoost;

    //TODO Remove, get value from pM
    [SerializeField] private float _deb;

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
        _energyLevel = Mathf.Clamp(_energyLevel + (_energyRegen - _energyCoost.Evaluate(_playerMovements.Speed.magnitude/_deb))*Time.deltaTime, 0f, 100f);
    }


}
