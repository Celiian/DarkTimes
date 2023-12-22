using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    [SerializeField] private EnemyHitController m_Assassin;
    [SerializeField] private SpawnerBoss m_Spawner;
    [SerializeField] private PlayerMovementController m_player;
    [SerializeField] private TMP_Text m_TextField;

    private string[] textsToDisplay;
    private int currentTextIndex = 0;
    private bool showingText = false;
    private bool _first = true;
    private bool _allDisplayed = false;

    void Start()
    {
        textsToDisplay = new string[]
        {
            "Well done, my child. You have vanquished the assassin, exacting justice for your clan. Your strength and resolve are commendable.",
            "Now, with the assassin's fall, our pact is fulfilled. You have avenged your kin and upheld your part of our ancient agreement.",
            "You shalt die."
        };
    }

    void Update()
    {
        if (m_Assassin._dead && !showingText)
        {
            var enemies = FindObjectsOfType<EnemyHitController>();
            foreach (var enemy in enemies)
            {
                m_Spawner.m_Stop = true;
                enemy.m_HitPoint = 0;
            }

            if (_allDisplayed)
            {
                return;
            }

            m_player._wait = true;

            StartCoroutine(DisplayTextFor3Seconds());
        }
    }

    private IEnumerator DisplayTextFor3Seconds()
    {
        showingText = true;

        if (_first)
        {
            yield return new WaitForSeconds(3f);

            m_player.transform.position = new Vector2(-1.424f, -3.476f);
            m_player.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            _first = false;
        }

        m_TextField.text = textsToDisplay[currentTextIndex];

        yield return new WaitForSeconds(5f); 

        m_TextField.text = "";

        currentTextIndex++;

        if (currentTextIndex >= textsToDisplay.Length)
        {
            currentTextIndex = 0;
            yield return new WaitForSeconds(0.5f); 
            AllTextsDisplayedFunction();
        }

        showingText = false;
    }

    private void AllTextsDisplayedFunction()
    {
        _allDisplayed = true;
        m_player.GetComponent<PlayerActionsController>().die();
        Invoke(nameof(menu), 7f);
    }


    private void menu()
    {
        SceneManager.LoadScene(0);
    }
}
