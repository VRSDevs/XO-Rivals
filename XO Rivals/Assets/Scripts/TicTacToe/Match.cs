using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class Match
{
    #region Vars

    
    /// <summary>
    /// ID de la partida
    /// </summary>
    public string MatchId { get; set; }
    /// <summary>
    /// ID del oponente
    /// </summary>
    public string OpponentId { get; set; }
    /// <summary>
    /// Nombre del jugador O
    /// </summary>
    public string PlayerOName { get; set; }
    /// <summary>
    /// Nombre del jugador X
    /// </summary>
    public string PlayerXName { get; set; }
    /// <summary>
    /// Nombre del jugador actual
    /// </summary>
    public string WhosTurn { get; set; }
    /// <summary>
    /// Momento de la partida en la que se encuentra el jugador
    /// </summary>
    public int TurnMoment { get; set; }
    /// <summary>
    /// ID del minijuego seleccionado
    /// </summary>
    public int MiniGameChosen { get; set; }
    /// <summary>
    /// Número de fichas colocadas en el tablero
    /// </summary>
    public int[,] FilledPositions { get; set; }
    /// <summary>
    /// Array del tablero
    /// </summary>
    public int NumFilled { get; set; }
    /// <summary>
    /// Array del tablero
    /// </summary>
    public int ChosenPosition { get; set; }
    /// <summary>
    /// Lista de fichas
    /// </summary>
    public List<GameObject> Chips { get; set; }
    public int ActualChip { get; internal set; }
    public string ActualChipTeam { get; internal set; }

    /// <summary>
    /// ¿Terminó la partida?
    /// </summary>
    private bool _ended;
    /// <summary>
    /// ¿Te rendiste?
    /// </summary>
    private bool _youSurrended;
    
    #endregion

    #region Constructors

    /// <summary>
    /// Constructor vacío.
    /// </summary>
    public Match()
    {
        MatchId = PhotonNetwork.CurrentRoom.Name;
        OpponentId = "";
        PlayerOName = "";
        PlayerXName = "";
        WhosTurn = "";
        TurnMoment = 0;
        MiniGameChosen = Random.Range(0,5);

        FilledPositions = new int[3, 3];
        for (int i = 0; i < FilledPositions.GetLength(0); i++)
        {
            for (int j = 0; j < FilledPositions.GetLength(1); j++)
            {
                FilledPositions[i, j] = 3;
            }
        }

        NumFilled = 0;

        Chips = new List<GameObject>();

        _ended = false;
        _youSurrended = false;
    }
    
    #endregion

    #region Getters

    /// <summary>
    /// Método para obtener si la partida ha finalizado o no
    /// </summary>
    /// <returns></returns>
    public bool IsEnded()
    {
        return _ended;
    }

    /// <summary>
    /// Método para obtener si el jugador local se rindió o no
    /// </summary>
    /// <returns></returns>
    public bool SurrenderStatus()
    {
        return _youSurrended;
    }

    #endregion

    #region Setters

    /// <summary>
    /// Método para establecer el valor de si la partida ha terminado o no
    /// </summary>
    public void SetIsEnded()
    {
        _ended = !_ended;
    }

    /// <summary>
    /// Método para establecer el valor de si el jugador local se ha rendido o no
    /// </summary>
    public void SetSurrenderStatus()
    {
        _youSurrended = !_youSurrended;
    }
    
    #endregion
}
