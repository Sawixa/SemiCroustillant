using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleScript : MonoBehaviour
{
    //private Collider2D _colliderCollectible;
    
    [SerializeField] private float _bonus;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerMovements playMov = collision.gameObject.GetComponent<PlayerMovements>();
        if (playMov != null) // we found the player
        {
            //Debug.Log("We found the player !");
            Energy nrj = collision.gameObject.GetComponent<Energy>();

            nrj.add(_bonus);

            Destroy(gameObject);

        }
    }

}
