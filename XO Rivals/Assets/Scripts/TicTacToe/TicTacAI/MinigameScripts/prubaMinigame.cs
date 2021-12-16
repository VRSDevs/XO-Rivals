using UnityEngine.SceneManagement;
using UnityEngine;

public class prubaMinigame : MonoBehaviour
{

    MatchAI thisMatch;
    
    void Start()
    {
        thisMatch = FindObjectOfType<MatchAI>();
        PlayerPrefs.SetInt("minigameWin", 1);
        thisMatch.TurnMoment = 2;
        SceneManager.LoadScene("TicTac_AI");
    }
}
