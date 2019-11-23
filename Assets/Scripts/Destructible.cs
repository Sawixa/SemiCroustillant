using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Destructible : MonoBehaviour
{
    public GameObject player;
    private PlayerMovements _playerMvt;

    private Collider2D _colliderDestructible;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Debug.Assert(player != null);
        _playerMvt = player.GetComponent<PlayerMovements>();
        Debug.Assert(_playerMvt);

        _colliderDestructible = GetComponent<Collider2D>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject == player)
        {
            if (_playerMvt.IsDashing)
            {
                _colliderDestructible.enabled = false;
                // TODO Destroy wall
            }

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
