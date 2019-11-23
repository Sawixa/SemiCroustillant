using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Destructible : MonoBehaviour
{
    private Collider2D _colliderDestructible;

    // Start is called before the first frame update
    void Start()
    {
        _colliderDestructible = GetComponent<Collider2D>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerMovements playerMovements = collision.gameObject.GetComponent<PlayerMovements>();
        if (playerMovements != null)
        {
            if (playerMovements.IsDashing)
            {
                _colliderDestructible.enabled = false;
                // TODO Destroy wall
            }
        }
    }
}
