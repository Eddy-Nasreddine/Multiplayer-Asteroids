using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    //public Text pointsText;
    public TextMeshProUGUI pointsText; // Change to TextMeshProUGUI


    public void Setup(int score)
    {
        gameObject.SetActive(true);
        pointsText.text = score.ToString() + " POINTS";

    }
    public void restartGame()
    {
        SceneManager.LoadScene("game");
    }

    public void goMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
