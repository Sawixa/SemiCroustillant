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

    private float _timeSinceBroken;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(damagedSprite != null);
        Debug.Assert(nbHitsToDestroy > 0);
        _colliderDestructible = GetComponent<Collider2D>();
        _sprRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        _timeSinceBroken += Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        BreakOnPlayerDash(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        BreakOnPlayerDash(collision);
    }

    private void BreakOnPlayerDash(Collision2D collision)
    {
        PlayerMovements playerMovements = collision.gameObject.GetComponent<PlayerMovements>();
        if (playerMovements != null)
        {
            if (playerMovements.IsDashing && _timeSinceBroken > playerMovements.DashLength)
            {
                _timeSinceBroken = 0f;
                _nbHits++;
                if (_nbHits == 1)
                {
                    _sprRenderer.sprite = damagedSprite;
                    AudioManager.PlaySFX("Arbre_fissure");
                }
                if (_nbHits >= nbHitsToDestroy)
                {
                    _colliderDestructible.enabled = false;
                    _sprRenderer.enabled = false;
                    Animator animSmoke = GetComponentInChildren<Animator>();
                    AudioManager.PlaySFX("Arbre_detruit");
                    //animSmoke.SetTrigger("Destroy");
                    // TODO Destroy wall
                }

            }
        }
    }
}
