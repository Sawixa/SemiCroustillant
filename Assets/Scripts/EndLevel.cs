using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovements>() != null)
            Skip();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0))
            Skip();
    }

    private void Skip()
    {
        SceneManager.LoadScene(2);
    }
}
