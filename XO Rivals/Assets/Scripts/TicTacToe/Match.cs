using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Match
{
    #region Vars

    /*
    /// <summary>
    /// ID de la partida
    /// </summary>
    public string MatchId { get; set; }
    
    /// <summary>
    /// ID del dueño de la partida
    /// </summary>
    public string OwnerId { get; set; }*/
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
    /// Lista de las fichas
    /// </summary>
    public List<GameObject> Chips { get; set; }
    // <summary>
    /// Ficha actual
    /// </summary>
    public int ActualChip { get; set; }
    public string ActualChipTeam { get; set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor vacío.
    /// </summary>
    public Match()
    {
        //MatchId = "";
        //OwnerId = "";
        PlayerOName = "";
        PlayerXName = "";
        WhosTurn = "";
        TurnMoment = 0;
        MiniGameChosen = Random.Range(0,2);

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

    }
    
    #endregion

    #region MyRegion

    

    #endregion
    
    
}
