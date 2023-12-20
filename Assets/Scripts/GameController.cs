using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private Text countdownText;
    [SerializeField] public float m_countdownTime;
    [SerializeField] private GameObject m_PlayerController;

    private Coroutine countdownCoroutine;
    private bool _accelerate;
    private GameManager _gameManager;

    private void Start()
    {
        countdownCoroutine = StartCoroutine(StartCountdown());

        _gameManager = FindObjectOfType<GameManager>();


        if(_gameManager == null)
        {

            var controller = new GameObject();

            _gameManager = controller.AddComponent<GameManager>();

            DontDestroyOnLoad(controller);
        }
      
    }

    private IEnumerator StartCountdown()
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

            if(m_countdownTime < 0)
            {
                m_countdownTime = 0;
            }

            int minutes = Mathf.FloorToInt(m_countdownTime / 60);
            int seconds = Mathf.FloorToInt(m_countdownTime % 60);
            int milliseconds = Mathf.FloorToInt((m_countdownTime * 100) % 100);

           
            countdownText.text = string.Format("{0}:{1:00}.{2:00}", minutes, seconds, milliseconds);


            yield return new WaitForSeconds(0.01f);
        }
        if(m_countdownTime <= 0)
        {
            m_countdownTime = 0;
            m_PlayerController.GetComponent<PlayerActionsController>().die();
        }

        countdownText.text = "0:00.00";
    }

    private void Update()
    {
    }

    public void PauseTimer(bool play)
    {
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
        m_countdownTime -= decount;

        if (m_countdownTime <= 0)
        {
            m_countdownTime = 0;
            m_PlayerController.GetComponent<PlayerActionsController>().die();
        }
        int minutes = Mathf.FloorToInt(m_countdownTime / 60);
        int seconds = Mathf.FloorToInt(m_countdownTime % 60);
        int milliseconds = Mathf.FloorToInt((m_countdownTime * 100) % 100);


        countdownText.text = string.Format("{0}:{1:00}.{2:00}", minutes, seconds, milliseconds);
    }

    public void addTime(float decount)
    {
        m_countdownTime += decount;

        int minutes = Mathf.FloorToInt(m_countdownTime / 60);
        int seconds = Mathf.FloorToInt(m_countdownTime % 60);
        int milliseconds = Mathf.FloorToInt((m_countdownTime * 100) % 100);


        countdownText.text = string.Format("{0}:{1:00}.{2:00}", minutes, seconds, milliseconds);
    }

    public void AccelerateTimer(bool accelerate)
    {
        _accelerate = accelerate;
    }
}
