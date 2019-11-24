using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    [SerializeField] GameObject _endDialog;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovements>() != null)
        {
            AudioManager.PlaySFX("Porte");
            _endDialog.SetActive(true);
            Invoke("Skip", 2);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            _endDialog.SetActive(true);
            Invoke("Skip", 2);
        }
    }

    private void Skip()
    {
        SceneManager.LoadScene(2);
    }
}
