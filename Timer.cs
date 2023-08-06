using System.Collections;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private TMP_Text timerText;

    public float timeLeft { get; set; } 

    public void StartTimer(float timeDuration)
    {
        timeLeft = timeDuration;
        StartCoroutine("TimerCoroutine");
    }
    public void StopTimer()
    {
        StopCoroutine("TimerCoroutine");
    }
    IEnumerator TimerCoroutine()
    {
        while (timeLeft > 1)
        {
            timeLeft -= Time.deltaTime;
            timerText.text = Mathf.FloorToInt(timeLeft) + " SEC";

            // ���� �����ӱ��� ��ٸ�
            yield return null; 
        }

        TimeOut();
    }

    public void AddTime(float sec)
    {
        timeLeft += sec;
    }
    void TimeOut()
    {
        GetComponentInParent<GameModeSelector>().gameMode.FinishGame();
    }  
}
