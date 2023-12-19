using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Text countdownText;

    [SerializeField]
    private float countdownTime;

    private Coroutine countdownCoroutine;
    private bool _accelerate;

    private void Start()
    {
        countdownCoroutine = StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {

        while (countdownTime > 0)
        {
            if (_accelerate)
            {
                countdownTime -= 0.015f;
            }
            else
            {
                countdownTime -= 0.01f;
            }
            int minutes = Mathf.FloorToInt(countdownTime / 60);
            int seconds = Mathf.FloorToInt(countdownTime % 60);
            int milliseconds = Mathf.FloorToInt((countdownTime * 100) % 100);

           
            countdownText.text = string.Format("{0}:{1:00}.{2:00}", minutes, seconds, milliseconds);


            yield return new WaitForSeconds(0.01f);
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
        countdownTime -= decount;

        int minutes = Mathf.FloorToInt(countdownTime / 60);
        int seconds = Mathf.FloorToInt(countdownTime % 60);
        int milliseconds = Mathf.FloorToInt((countdownTime * 100) % 100);


        countdownText.text = string.Format("{0}:{1:00}.{2:00}", minutes, seconds, milliseconds);
    }

    public void AccelerateTimer(bool accelerate)
    {
        _accelerate = accelerate;
    }
}
