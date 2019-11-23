using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Destructible : MonoBehaviour
{
    public GameObject player;

    private PlayerMovements _playerMov;

    private Collider2D _collider;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Debug.Assert(player != null);
        if (player != null)
        {
            _playerMov = player.GetComponent<PlayerMovements>();
            Debug.Assert(_playerMov != null);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
