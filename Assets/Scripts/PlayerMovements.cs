using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector2 _speed;

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

    void Move(Vector2 delta)
    {
        //TODO
        
    }

    void Dash()
    {
        //TODO
    }

    /*
     * Uograde speed
     */
    void UpGear(float delta_time)
    {
        //TODO
    }

    /*
     * downgrade speed
     */
    void DownGear(float delta_time)
    {
        //TODO
    }
}
