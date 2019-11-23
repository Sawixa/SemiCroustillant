using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour
{

    [SerializeField, Range(0,100)] private float _energyLevel;

    [SerializeField] private float energyRegen;
    [SerializeField] private float energyCost;


    // Start is called before the first frame update
    void Start()
    {
        _energyLevel = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        _energyLevel = Mathf.Clamp(_energyLevel + (energyRegen - energyCost)*Time.deltaTime, 0f, 100f);
    }


}
