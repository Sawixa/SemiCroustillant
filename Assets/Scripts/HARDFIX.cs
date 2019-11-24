using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HARDFIX : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject cutscene;
    void Start()
    {
        StartCoroutine(DestroyCutscene());
    }

    IEnumerator DestroyCutscene()
    {
        yield return new WaitForSeconds(6f);
        player.SetActive(false);
        Destroy(cutscene);
        yield return new WaitForSeconds(.001f);
        player.SetActive(true);
    }
}
