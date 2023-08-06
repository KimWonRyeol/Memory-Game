using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetCardPanel : MonoBehaviour
{
    [SerializeField]
    private GridLayoutGroup gridLayoutGroup;

    public void UpdateCardPanel(int stage)
    {
        if(stage == 0)
        {
            //카드 개수에 맞게 카드 크기 조정 
            gridLayoutGroup.cellSize = new Vector2(500, 300);
            //스테이지에 맞게 카드 행당 카드 수 설정
            gridLayoutGroup.constraintCount = 2;
        }
        else if(stage == 1)
        {
            gridLayoutGroup.cellSize = new Vector2(500, 300);
            gridLayoutGroup.constraintCount = 4;
        }
        else if(stage >= 2)
        {
            gridLayoutGroup.cellSize = new Vector2(300, 200);
            gridLayoutGroup.constraintCount = 6;
        }
        else
        {
            Debug.Log("UpdateCardPanel: 올바르지 않은 Stage 정보");
        }
    }
}
