using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public void startGame(){
        SceneManager.LoadScene("GameScene");
    }

    public void quitGame(){
        Application.Quit();
    }
}
