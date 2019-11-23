using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;
    int _sceneToLoad;
    bool _canLoadGame = false;

    void Start()
    {
        _sceneToLoad = SceneManager.GetActiveScene().buildIndex + 1;
        if (_sceneToLoad == 1)
        {
            StartCoroutine(StartGame());
        }
        else if (_sceneToLoad <= SceneManager.sceneCountInBuildSettings)
        {
            _canLoadGame = true;
            //_anim.SetTrigger("FadeIn");
        }
        else
        {
            _sceneToLoad = 0;
        }
    }

    IEnumerator StartGame()
    {
        Animator FadeOut = GameObject.Find("StartMenu").GetComponent<Animator>();
        yield return new WaitUntil(() => Input.anyKey);
        FadeOut.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1.25f);
        SceneManager.LoadScene(_sceneToLoad);
    }

    public void Load()
    {
        StartCoroutine(LoadGame());
    }

    IEnumerator LoadGame()
    {
        if (_canLoadGame)
        {
            //_anim.SetTrigger("FadeOut");
            yield return new WaitForSeconds(1.25f);
            SceneManager.LoadScene(_sceneToLoad);

        }
    }
}
