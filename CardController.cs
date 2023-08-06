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
        
        //���ο��� �� ī�� �޸����� ����
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

    // Instantiate�� ������Ʈ �±׷� ã�Ƽ� ����Ʈ�� �Ҵ�
    void GetCardButtons()
    {
        GameObject[] cardObjects = GameObject.FindGameObjectsWithTag("Card");

        //ī�� ������Ʈ���� ��ư ������Ʈ�� ����Ʈ�� ����, ī�� Default �̹��� ����
        for (int i = 0; i < cardObjects.Length; i++)
        {
            cardBtns.Add(cardObjects[i].GetComponent<Button>());
            cardBtns[i].image.sprite = backCardImg;
        }

        AddListners();
    }

    //card�� OnClick Event �Ҵ�
    void AddListners()
    {
        foreach (Button btn in cardBtns)
        {
            btn.onClick.AddListener(delegate { GetComponentInParent<GameModeSelector>().gameMode.PickUpCard(btn); });
        }
    }

    public void SetRandomCards()
    {
        //���� ī�� ��ȣ�� �ߺ����� �ʱ� ���� HashSet�� ����
        HashSet<int> selectedNumbers = new HashSet<int>();
        int randomNum;

        //ī�带 �������� ����(��ü ī�� ���� ����, ¦�� ���߱� ����)
        while (selectedNumbers.Count < cardCount / 2)
        {
            randomNum = Random.Range(0, 52);
            selectedNumbers.Add(randomNum);
        }

        //���� ī�带 2�徿 �߰�
        foreach (int selectedNum in selectedNumbers)
        {
            cards.Add(selectedNum);
            cards.Add(selectedNum);
        }

        //���� ī��� Shuffle
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

    //Card Objects ��� ����
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
