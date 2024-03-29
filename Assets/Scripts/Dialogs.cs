﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Dialogs : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textDisplay;
    [SerializeField] string[] _sentences;
    [SerializeField] GameObject[] _speakers;
    [SerializeField] int[] _sentenceSpeaker;

    int _index;
    [SerializeField] float _typingSpeed = .045f;

    [SerializeField] GameObject _continueButton;
    Animator _dialogAnimator;

    [SerializeField] bool _isEndDialog = false;


    private void Start()
    {
        _dialogAnimator = GetComponent<Animator>();
        _textDisplay.text = "";
        StartCoroutine(Type());
    }


    void Update()
    {
        if (_textDisplay.text == _sentences[_index])
        {
            // TODO Wait for typing end
            _continueButton.SetActive(true);
        }
    }

    IEnumerator Type()
    {
        yield return new WaitForSeconds(_typingSpeed * 10);
        foreach (char letter in _sentences[_index].ToCharArray())
        {
            _textDisplay.text += letter;
            yield return new WaitForSeconds(_typingSpeed);
        }
    }


    public void NextSentence()
    {
        // SFX ?
        _dialogAnimator.SetTrigger("ShowText");
        _continueButton.SetActive(false);

        if (_index < _sentenceSpeaker.Length)
        {
            foreach (GameObject g in _speakers)
            {
                g.SetActive(false);
            }
            _speakers[_sentenceSpeaker[_index]].SetActive(true);
        }

        if (_index < _sentences.Length - 1)
        {
            _index++;
            _textDisplay.text = "";
            StartCoroutine(Type());
        }
        else
        {
            if (!_isEndDialog)
            {
                _textDisplay.text = "";
                _dialogAnimator.SetTrigger("Disappear");

                Invoke("Hide", 1.2f);

            }
            else
            {
                _dialogAnimator.SetTrigger("Fade");
                Invoke("LoadNextLvl", 2.5f);
            }
        }
    }

    void ChangeSpeaker(int id)
    {
        _speakers[id].SetActive(true);
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }

    void LoadNextLvl()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
