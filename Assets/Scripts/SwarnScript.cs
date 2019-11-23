using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarnScript : EnnemyIA
{
    protected override void Turn()
    {
        Vector2 newDir = ((Vector2)_playerLight.transform.position - Position);

        float angle = Vector2.SignedAngle(_dir, newDir);
        if (angle != 0)
        {
            _dir = Vector3.Lerp(_dir, newDir, Mathf.Abs(_step / angle)).normalized;
        }
    }
}
