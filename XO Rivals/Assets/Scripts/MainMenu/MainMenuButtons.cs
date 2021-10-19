using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public void goToGame(){
        SceneManager.LoadScene("Tic Tac Toe");
    }

    public void goToSettings(){
        SceneManager.LoadScene("Tic Tac Toe");
    }

    public void goToCredits(){
        SceneManager.LoadScene("Tic Tac Toe");
    }
}
