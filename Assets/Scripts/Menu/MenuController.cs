using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    FadeInOut fade;
    [SerializeField] bool isGameOver;
    public void Start()
    {
        fade = FindObjectOfType<FadeInOut>();
    }
    private IEnumerator PlayGame()
    { 
        fade.FadeIn();
        yield return new WaitForSeconds(1);
        int levelMap = isGameOver ? 2 : 1;
        SceneManager.LoadSceneAsync(levelMap);
    }

    public void PressPlayGame()
    {
        StartCoroutine(PlayGame());
    }

    public void QuitGame()
    {
        Debug.Log("quit game");
        Application.Quit();
    }
}