using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TicTacToeController : MonoBehaviour
{
    private GameManager _gameManager;
    
    public AudioClip Tic_Tac_toe_Music;
    
    [SerializeField] public TMP_Text PlayerCounter;

    #region UnityCB

    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        
        FindObjectOfType<AudioManager>().StopAllSongs();
        //FindObjectOfType<AudioManager>().Stop("Main_Menu");

        //FindObjectOfType<AudioManager>().Play("Tic-Tac-Toe");

        FindObjectOfType<AudioManager>().ChangeMusic(Tic_Tac_toe_Music,"Main_menu");

    }
    
    void FixedUpdate()
    {
        PlayerCounter.text = PhotonNetwork.CurrentRoom.PlayerCount + " / " + PhotonNetwork.CurrentRoom.MaxPlayers;
    }

    #endregion

    #region OtherMethods

    /// <summary>
    /// Método para volver al menú principal
    /// </summary>
    public void GoBack()
    {
        _gameManager.OnLeaveRoom();
        SceneManager.LoadScene("MainMenu");
    }

    public void PutChip()
    {
        FindObjectOfType<AudioManager>().Play("Chip");
    }
    
    public void SelectButton()
    { 
        FindObjectOfType<AudioManager>().Play("SelecctionButton1");
    }

    /// <summary>
    /// Método para abandonar la partida
    /// </summary>
    public void Surrender()
    {
        FindObjectOfType<GameManager>().PlayerMatches[PhotonNetwork.CurrentRoom.Name].SetSurrenderStatus();
        _gameManager._networkCommunications.SendEndMatchInfo("SR", "");
        _gameManager.OnLeaveRoom();
        SceneManager.LoadScene("MainMenu");
    }

    #endregion
    
}
