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
            //ī�� ������ �°� ī�� ũ�� ���� 
            gridLayoutGroup.cellSize = new Vector2(500, 300);
            //���������� �°� ī�� ��� ī�� �� ����
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
            Debug.Log("UpdateCardPanel: �ùٸ��� ���� Stage ����");
        }
    }
}
