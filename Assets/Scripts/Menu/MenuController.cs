using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    FadeInOut fade;

    public void Start()
    {
        fade = FindObjectOfType<FadeInOut>();
    }
    private IEnumerator PlayGame()
    {
        
        fade.FadeIn();
        yield return new WaitForSeconds(1);
        SceneManager.LoadSceneAsync(1);
            
 
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