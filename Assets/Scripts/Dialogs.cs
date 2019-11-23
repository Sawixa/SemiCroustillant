using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialogs : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textDisplay;
    [SerializeField] string[] _sentences;
    [SerializeField] GameObject[] _speakers;
    int _index;
    [SerializeField] float _typingSpeed = .03f;

    [SerializeField] GameObject _continueButton;
    [SerializeField] GameObject _flash;
    [SerializeField] Animator _displayTextAnim;


    private void Start()
    {
        StartCoroutine(Type());
    }


    void Update()
    {
        if (_textDisplay.text == _sentences[_index])
        {
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
        _displayTextAnim.SetTrigger("ShowText");
        _continueButton.SetActive(false);

        if (_index < _sentences.Length - 1)
        {
            _index++;
            _textDisplay.text = "";
            StartCoroutine(Type());
        }
        else
        {
            _textDisplay.text = "";
            _flash.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
