﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovements))]
public class InputController : MonoBehaviour
{
    float _horizontalInput;
    float _verticalInput;
    PlayerMovements _playerMovements;

    bool _canDash;
    float _dashCD;
    float _timer;
    float __speedTransitionTime;

    [Tooltip("Menu de pause")]
    [SerializeField] GameObject _pausePanel;
    bool _pauseState = false;


    private void Awake()
    {
        _playerMovements = GetComponent<PlayerMovements>();
    }

    void Update()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
        Vector2 input = new Vector2(_horizontalInput, _verticalInput);

        _playerMovements.Move(input);

        // Gear Up
        if (Input.GetButtonDown("GearUp"))
        {
            _playerMovements.UpGear(__speedTransitionTime);
        }
        // Gear Down
        if (Input.GetButtonDown("GearDown"))
        {
            _playerMovements.DownGear(__speedTransitionTime);
        }

        if (Input.GetButtonDown("Pause"))
        {
            _pauseState = !_pauseState;
            Pause(_pauseState);
        }


        //Dash
        if (Input.GetButton("Dash") && _timer < 0)
        {
            _playerMovements.Dash();
        }

    }

    void Pause(bool pauseState)
    {
        Time.timeScale = pauseState ? 0 : 1;
        _pausePanel.SetActive(_pauseState);
    }

}