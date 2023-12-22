using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private GameObject m_PlayerController;
    [SerializeField] private bool m_Tuto;


    public FadeInOut fade;
    public float m_countdownTime;
    private Coroutine countdownCoroutine;
    private bool _accelerate;
    private GameManager _gameManager;
    private float _startingtime = 300;

    private void Start()
    {

        fade = FindObjectOfType<FadeInOut>();

        _gameManager = FindObjectOfType<GameManager>();


            if(_gameManager == null)
            {

                var controller = new GameObject();

                _gameManager = controller.AddComponent<GameManager>();

                DontDestroyOnLoad(controller);
                _gameManager.countDown = _startingtime;

            }

        m_countdownTime = _gameManager.countDown;

        
         countdownCoroutine = StartCoroutine(StartCountdown());
        

    }

    public IEnumerator StartCountdown()
    {

        if (!m_Tuto)
        {

            while (m_countdownTime > 0)
            {
                if (_accelerate)
                {
                    m_countdownTime -= 0.015f;
                }
                else
                {
                    m_countdownTime -= 0.01f;
                }

                if (m_countdownTime < 0)
                {
                    m_countdownTime = 0;
                }

                int minutes = Mathf.FloorToInt(m_countdownTime / 60);
                int seconds = Mathf.FloorToInt(m_countdownTime % 60);
                int milliseconds = Mathf.FloorToInt((m_countdownTime * 100) % 100);


                countdownText.text = string.Format("{0}:{1:00}.{2:00}", minutes, seconds, milliseconds);

                _gameManager.countDown = m_countdownTime;
                yield return new WaitForSeconds(0.01f);
            }
            if (m_countdownTime <= 0)
            {
                m_countdownTime = 0;

                Invoke(nameof(GameOver), 2.5f);

                m_PlayerController.GetComponent<PlayerActionsController>().die();

            }

            countdownText.text = "0:00.00";
        }
        
    }

    private void GameOver()
    {
        _gameManager.countDown = _startingtime;
        fade.FadeIn();
        Invoke(nameof(LoadScene),1);
        ;
    }
    private void LoadScene()
    {
        SceneManager.LoadScene(6);
    }


    public void PauseTimer(bool play)
    {
        if (m_Tuto)
        {
            return;
        }
        if (play)
        {
            if (countdownCoroutine == null)
            {
                countdownCoroutine = StartCoroutine(StartCountdown());
            }
        }
        else
        {
            if (countdownCoroutine != null)
            {
                StopCoroutine(countdownCoroutine);
                countdownCoroutine = null;
            }
        }
    }


    public void DecountTimer(float decount)
    {
        if (m_Tuto)
        {
            return;
        }
            m_countdownTime -= decount;

        _gameManager.countDown = m_countdownTime;
        if (m_countdownTime <= 0)
        {
            m_countdownTime = 0;
            Invoke(nameof(GameOver), 3);
            m_PlayerController.GetComponent<PlayerActionsController>().die();
        }
        int minutes = Mathf.FloorToInt(m_countdownTime / 60);
        int seconds = Mathf.FloorToInt(m_countdownTime % 60);
        int milliseconds = Mathf.FloorToInt((m_countdownTime * 100) % 100);


        countdownText.text = string.Format("{0}:{1:00}.{2:00}", minutes, seconds, milliseconds);
    }

    public void addTime(float decount)
    {
        if (m_Tuto)
        {
            return;
        }

        m_countdownTime += decount;
        _gameManager.countDown = m_countdownTime;
        int minutes = Mathf.FloorToInt(m_countdownTime / 60);
        int seconds = Mathf.FloorToInt(m_countdownTime % 60);
        int milliseconds = Mathf.FloorToInt((m_countdownTime * 100) % 100);


        countdownText.text = string.Format("{0}:{1:00}.{2:00}", minutes, seconds, milliseconds);
    }

    public void AccelerateTimer(bool accelerate)
    {
        if (m_Tuto)
        {
            return;
        }
        _accelerate = accelerate;
    }
}
