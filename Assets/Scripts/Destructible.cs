using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Destructible : MonoBehaviour
{
    private Collider2D _colliderDestructible;
    private SpriteRenderer _sprRenderer;

    public Sprite damagedSprite;

    public int nbHitsToDestroy;
    private int _nbHits = 0;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(damagedSprite != null);
        Debug.Assert(nbHitsToDestroy > 0);
        _colliderDestructible = GetComponent<Collider2D>();
        _sprRenderer = GetComponent<SpriteRenderer>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerMovements playerMovements = collision.gameObject.GetComponent<PlayerMovements>();
        if (playerMovements != null)
        {
            if (playerMovements.IsDashing)
            {
                _nbHits++;
                if (_nbHits == 1)
                {
                    _sprRenderer.sprite = damagedSprite;
                }
                if (_nbHits >= nbHitsToDestroy)
                {
                    _colliderDestructible.enabled = false;
                    _sprRenderer.enabled = false;
                    Animator animSmoke = GetComponentInChildren<Animator>();
                    animSmoke.SetTrigger("Destroy");
                    // TODO Destroy wall
                }

            }
        }
    }
}
