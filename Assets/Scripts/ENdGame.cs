using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ENdGame : MonoBehaviour
{
    [SerializeField] GameObject _dialogue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _dialogue.SetActive(true);
    }
}
