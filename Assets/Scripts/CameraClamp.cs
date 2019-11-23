using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraClamp : MonoBehaviour
{
    [SerializeField] float _yCeilingClamp = 10f;
    [SerializeField] float _yFloorClamp = -10f;
    [SerializeField] float _xClamp = -5f;


    void Update()
    {
        float yPos = transform.position.y;
        float xPos = transform.position.x;

        if (yPos >= _yCeilingClamp)
        {
            yPos = _yCeilingClamp;
        }
        else if (transform.position.y <= _yFloorClamp)
        {
            yPos = _yCeilingClamp;
        }

        if (xPos <= _xClamp)
        {
            xPos = _xClamp;
        }
        transform.position = new Vector2(xPos, yPos);
    }
}
