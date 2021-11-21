using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TicTacToeController : MonoBehaviour
{
    private GameManager _gameManager;

    [SerializeField] public TMP_Text PlayerCounter;

    #region UnityCB

    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
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
        _gameManager.IsPlaying = false;
        _gameManager.OnLeaveRoom();
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Método para abandonar la partida
    /// </summary>
    public void Surrender()
    {
        _gameManager.IsPlaying = false;
        FindObjectOfType<GameManager>().PlayerMatches[PhotonNetwork.CurrentRoom.Name].SetSurrenderStatus();
        _gameManager._networkCommunications.SendEndMatchInfo("SR", "");
        _gameManager.OnLeaveRoom();
        SceneManager.LoadScene("MainMenu");
    }

    #endregion
    
}
