using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    [HideInInspector] public Vector2 speed;

    //private Vector2 _position;
    public Vector2 Position
    {
        get
        {
            return transform.position;
        }

        set
        {
            transform.position = new Vector3(value.x, value.y, transform.position.z);
        }
    }

    [SerializeField] private float[] _maxSpeeds = new float[3];
    private int _gear; // 0 1 or 2
    
     void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(Vector2 delta)
    {
        //TODO
        
    }

    public void Dash()
    {
        //TODO
    }

    /*
     * Upgrade speed
     */
    public void UpGear(float delta_time)
    {
        //TODO
    }

    /*
     * downgrade speed
     */
    public void DownGear(float delta_time)
    {
        //TODO
    }
}
