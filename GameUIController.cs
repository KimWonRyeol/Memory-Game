using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text stageText;

    [SerializeField]
    private TMP_Text poinText;

    [SerializeField]
    private Button bgmToggleBtn;
    [SerializeField]
    private Button backBtn;


    private Image offImg;

    private void Awake()
    {
        //전 씬의 isMuted 설정 적용
        offImg = bgmToggleBtn.gameObject.transform.GetChild(0).GetComponent<Image>();
        GameManager.Instance.isMuted = !GameManager.Instance.isMuted;
        ToggleMuteButton();

        //뒤로가기 
        backBtn.onClick.AddListener(delegate { GoToLobby(); });

        //bgm On/Off 
        bgmToggleBtn.onClick.AddListener(delegate { ToggleMuteButton(); });
        
    }


    public void UpdateStageText(int stage)
    {
        stageText.text = "Stage " + stage;
    }
    public void UpdatePointText(int point)
    {
        poinText.text = point.ToString();
    }


    public void GoToLobby()
    {
        AudioManager.Instance.PlaySFX("ClickButton");

        SceneManager.LoadScene("MainLobby");
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
