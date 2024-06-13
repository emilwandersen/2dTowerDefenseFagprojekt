using UnityEngine;
using UnityEngine.UI;

public class ShopTower : MonoBehaviour
{
 public Player playerRef;
 public Tower newTower;
 public Button myButton;

 
  public void ChangeToRedTower(){
    newTower.SetPrefab("RangedRedTower");
    playerRef.SetTower(newTower);
  }

  public void ChangeToWhiteTower(){
        newTower.SetPrefab("RangedWhiteTower");
        playerRef.SetTower(newTower);
  }
}
 