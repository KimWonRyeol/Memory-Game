using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CardController : MonoBehaviour
{
    [SerializeField]
    private Transform cardsPanel;
    [SerializeField]
    private GameObject cardPrefab;


    private Sprite[] frontCardImgs;
    private Sprite[] backCardImgs;
    private Sprite backCardImg;

    private List<int> cards = new List<int>();
    private List<Button> cardBtns = new List<Button>();

    private int cardCount;

    public void Awake()
    {
        frontCardImgs = Resources.LoadAll<Sprite>("CardImages/FrontImg");
        backCardImgs = Resources.LoadAll<Sprite>("CardImages/BackImg");
        
        //메인에서 고른 카드 뒷면으로 설정
        backCardImg = backCardImgs[GameManager.Instance.backCardSkin];
    }

    public int GetCard(int index)
    {
        return cards[index];
    }

    public void OpenCard(int index)
    {
        cardBtns[index].image.sprite = frontCardImgs[cards[index]];
        cardBtns[index].interactable = false;
    }
    public void CloseCard(int index)
    {
        cardBtns[index].image.sprite = backCardImg;
        cardBtns[index].interactable = true;
    }

    public void DisableCard(int index)
    {
        cardBtns[index].interactable = false;
        cardBtns[index].image.color = new Color(0, 0, 0, 0);
    }


    public void InstantiateCards(int cardCount_)
    {
        cardCount= cardCount_;

        for (int i = 0; i < cardCount; i++)
        {
            GameObject card = Instantiate(cardPrefab);
            card.name = i.ToString();
            card.transform.SetParent(cardsPanel, false);
        }

        GetCardButtons();
    }

    // Instantiate한 오브젝트 태그로 찾아서 리스트에 할당
    void GetCardButtons()
    {
        GameObject[] cardObjects = GameObject.FindGameObjectsWithTag("Card");

        //카드 오브젝트에서 버튼 컴포넌트들 리스트로 관리, 카드 Default 이미지 세팅
        for (int i = 0; i < cardObjects.Length; i++)
        {
            cardBtns.Add(cardObjects[i].GetComponent<Button>());
            cardBtns[i].image.sprite = backCardImg;
        }

        AddListners();
    }

    //card들 OnClick Event 할당
    void AddListners()
    {
        foreach (Button btn in cardBtns)
        {
            btn.onClick.AddListener(delegate { GetComponentInParent<GameModeSelector>().gameMode.PickUpCard(btn); });
        }
    }

    public void SetRandomCards()
    {
        //뽑은 카드 번호가 중복되지 않기 위해 HashSet에 저장
        HashSet<int> selectedNumbers = new HashSet<int>();
        int randomNum;

        //카드를 랜덤으로 뽑음(전체 카드 수의 절반, 짝을 맞추기 위해)
        while (selectedNumbers.Count < cardCount / 2)
        {
            randomNum = Random.Range(0, 52);
            selectedNumbers.Add(randomNum);
        }

        //뽑은 카드를 2장씩 추가
        foreach (int selectedNum in selectedNumbers)
        {
            cards.Add(selectedNum);
            cards.Add(selectedNum);
        }

        //뽑은 카드들 Shuffle
        Shuffle(cards);
    }

    //Knuth Shuffle
    void Shuffle(List<int> list)
    {
        int temp;
        int randomIndex;

        for (int i = list.Count - 1; i > 0; i--)
        {
            randomIndex = Random.Range(0, i + 1);
            temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    //Card Objects 모두 지움
    public void ClearCards()
    {
        foreach (Transform child in cardsPanel)
        {
            Destroy(child.gameObject);
        }

        cardBtns.Clear();
        cards.Clear();
    }

}
