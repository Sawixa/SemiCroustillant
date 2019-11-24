using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RinoScript : EnnemyIA
{
    [SerializeField] private ParticleSystem _deathFX;
    protected override void Turn()
    { 
        float angle = Vector2.SignedAngle(-transform.right, (Vector2)_playerLight.transform.position - Position);
        float zDir = (Mathf.Abs(angle) > _step) ? (transform.rotation.eulerAngles.z + Mathf.Sign(angle) * _step) : (transform.rotation.eulerAngles.z + angle);
        transform.rotation = Quaternion.Euler(0, 0, zDir);
        _dir = -transform.right;
    }

    public override void Die()
    {
        StartCoroutine(DieCoroutine());
        //Destroy(gameObject);
    }
    IEnumerator DieCoroutine()
    {
        SpriteRenderer spRd = GetComponent<SpriteRenderer>();
        Rigidbody2D rigBd = GetComponent<Rigidbody2D>();
        spRd.enabled = false;
        rigBd.simulated = false;
        _deathFX.Play();
        Debug.Log("play");
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);

    }
}
