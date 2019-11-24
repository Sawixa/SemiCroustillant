using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Experimental.Rendering.LWRP;

public class UpgradeMenu : MonoBehaviour
{

    [SerializeField] private GameObject _offensivePlayerPrefab;
    [SerializeField] private GameObject _defensivePlayerPrefab;
    [SerializeField] private GameObject _vanillaPlayerPrefab;
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private CinemachineVirtualCamera _cinemachine;

    private void Start()
    {
        Debug.Assert(_offensivePlayerPrefab != null);
        Debug.Assert(_defensivePlayerPrefab != null);
        Debug.Assert(_vanillaPlayerPrefab != null);
        Debug.Assert(_pausePanel != null);
        Debug.Assert(_spawnPoint != null);
        Debug.Assert(_cinemachine != null);
    }

    public void Offensive()
    {
        GameObject player = Instantiate(_offensivePlayerPrefab,_spawnPoint.position,_spawnPoint.rotation);
        _cinemachine.Follow = player.transform;
        player.GetComponent<InputController>().PausePanel = _pausePanel;
        Destroy(gameObject);
        Activate(player);
    }

    public void Defensive()
    {
        GameObject player = Instantiate(_defensivePlayerPrefab, _spawnPoint.position, _spawnPoint.rotation);
        _cinemachine.Follow = player.transform;
        player.GetComponent<InputController>().PausePanel = _pausePanel;
        Destroy(gameObject);
        Activate(player);
    }

    public void Vanilla()
    {
        GameObject player = Instantiate(_vanillaPlayerPrefab, _spawnPoint.position, _spawnPoint.rotation);
        _cinemachine.Follow = player.transform;
        player.GetComponent<InputController>().PausePanel = _pausePanel;
        Destroy(gameObject);
        Activate(player);
    }

    private void Activate(GameObject player)
    {
        Light2D playerLight = player.GetComponentInChildren<Light2D>();

        EnnemyIA[] ennemies = FindObjectsOfType<EnnemyIA>();
        for (int i = 0; i < ennemies.Length; ++i)
        {
            ennemies[i].PlayerLight = playerLight;
        }
    }
}
