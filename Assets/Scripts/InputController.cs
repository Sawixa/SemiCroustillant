using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovements))]
public class InputController : MonoBehaviour
{
    float _horizontalInput;
    float _verticalInput;
    PlayerMovements _playerMovements;
    private Energy _energy;

    bool _canDash; // maye be useless
    [SerializeField]float _dashCD; // dash cool down
    float _timeOfLastDash;
    float _speedTransitionTime;
    
    [SerializeField, Range(0, 100)] private float _dashCost;


    [Tooltip("Menu de pause")]
    [SerializeField] GameObject _pausePanel;

    bool _pauseState = false;


    private void Awake()
    {
        _playerMovements = GetComponent<PlayerMovements>();
        _energy = GetComponent<Energy>();
        _timeOfLastDash = Time.time;
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
            _playerMovements.UpGear(_speedTransitionTime);
        }
        // Gear Down
        if (Input.GetButtonDown("GearDown"))
        {
            _playerMovements.DownGear(_speedTransitionTime);
        }

        if (Input.GetButtonDown("Pause"))
        {
            _pauseState = !_pauseState;
            Pause(_pauseState);
        }


        //Dash
        if (Input.GetButton("Dash"))
        {
            float curTime = Time.time;
            if (curTime > _timeOfLastDash + _dashCD && _energy.energy >= _dashCost + 5)
            {
                _energy.add(-_dashCost);
                _playerMovements.Dash();
                _timeOfLastDash = curTime;
            }
            
        }

    }

    void Pause(bool pauseState)
    {
        Time.timeScale = pauseState ? 0 : 1;
        _pausePanel.SetActive(_pauseState);
    }

}
