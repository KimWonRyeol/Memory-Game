using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameSceneUIController : MonoBehaviour
{
    [SerializeField]
    private Button bgmToggleBtn;
    [SerializeField]
    private Button replayButton;
    [SerializeField]
    private TMP_Text pointText;
    [SerializeField]
    private TMP_Text stageBonusText;
    [SerializeField]
    private TMP_Text timeBonusText;
    [SerializeField]
    private TMP_Text totalBonusText;
    [SerializeField]
    private TMP_Text bestScoreText;
    [SerializeField]
    private Image gradeImage;

    private Sprite[] gradeSprites;


    private Image offImg;

    private void Awake()
    {
        gradeSprites = Resources.LoadAll<Sprite>("GradeImg");

        //전 씬의 isMuted 설정 적용
        offImg = bgmToggleBtn.gameObject.transform.GetChild(0).GetComponent<Image>();
        GameManager.Instance.isMuted = !GameManager.Instance.isMuted;
        ToggleMuteButton();

        bgmToggleBtn.onClick.AddListener(delegate { ToggleMuteButton(); });
        
    }

    public void ReplayGame()
    {
        AudioManager.Instance.PlaySFX("ClickButton");

        SceneManager.LoadScene("GameScene");
    }

    public void GoToLobby()
    {
        AudioManager.Instance.PlaySFX("ClickButton");

        SceneManager.LoadScene("MainLobby");
    }

    public void UpdatePointText(int point)
    {
        pointText.text = point.ToString();
    }

    public void UpdateStageBonusText(int stage, int stagePerBonus, int stageBonus)
    {
        stageBonusText.text = $"Stage bonus: \r\n{stage} X ({stagePerBonus}) = {stageBonus}\r\n";
    }

    public void UpdateTimeBonusText(int timeLeft, int timePerBonus, int timeBonus)
    {
        timeBonusText.text = $"Time bonus: \r\n{timeLeft} X ({timePerBonus}) = {timeBonus}\r\n";
    }

    public void UpdateTotalBonusText(int stageBonus, int timeBonus, int totalBonus)
    {
        totalBonusText.text = $"-----------------\r\nTotal bonus: \r\n{stageBonus} + {timeBonus} = {totalBonus}\r\n";
    }

    public void UpdateBestScoreText(int bestScore)
    {
        bestScoreText.text = "Best Score: " + bestScore.ToString();  
    }

    public void ToggleMuteButton()
    {
        AudioManager.Instance.PlaySFX("ClickButton");

        //Mute
        if (!GameManager.Instance.isMuted)
        {
            AudioListener.volume = 0f;
            offImg.color = new Color(255, 0, 0, 255);
            GameManager.Instance.isMuted = true;
        }
        else
        {
            AudioListener.volume = 1f;
            offImg.color = new Color(0, 0, 0, 0);
            GameManager.Instance.isMuted = false;
        }
    }
}
