using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IGameMode 
{
    public void StartGame();
    public void FinishGame();
    public void PickUpCard(Button btn);
}
