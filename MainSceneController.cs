using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainSceneController : MonoBehaviour
{
    [SerializeField]
    private Button bgmToggleBtn;

    private Image offImg;
    private void Start()
    {
        //전 씬의 isMuted 설정 적용
        offImg = bgmToggleBtn.gameObject.transform.GetChild(0).GetComponent<Image>();
        GameManager.Instance.isMuted = !GameManager.Instance.isMuted;
        ToggleMuteButton();

        bgmToggleBtn.onClick.AddListener(delegate { ToggleMuteButton(); });
    }

    public void EnterChallengeMode()
    {
        AudioManager.Instance.PlaySFX("ClickButton");

        GameManager.Instance.kindGameMode = KindGameMode.ChallengeMode;
        SceneManager.LoadScene("GameScene");
    }

    public void EnterstageMode()
    {
        AudioManager.Instance.PlaySFX("ClickButton");

        GameManager.Instance.kindGameMode = KindGameMode.StageMode;
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ToggleMuteButton()
    {
        AudioManager.Instance.PlaySFX("ClickButton");
        print(GameManager.Instance.isMuted);
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
