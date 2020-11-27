using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MenuManager
{

    void Update()
    {
        StartCoroutine(GoToMainMenu(5f));
    }

    IEnumerator GoToMainMenu(float time)
    {
        yield return new WaitForSeconds(time);
        GoToScene("MainMenu");
    }
}