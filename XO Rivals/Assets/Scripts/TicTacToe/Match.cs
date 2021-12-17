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
        MatchId = "";
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
        ActualChip = -1;
        ActualChipTeam = "";

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

    #region OtherMethods

    /// <summary>
    /// Método para incorporar el contenido de una partida en una cadena de texto
    /// </summary>
    /// <returns>Cadena de texto con la información de la partida</returns>
    public override string ToString()
    {
        string match = "";

        match += MatchId;
        match += ";";

        match += OpponentId;
        match += ";";

        match += PlayerOName;
        match += ";";

        match += PlayerXName;
        match += ";";

        match += WhosTurn;
        match += ";";

        match += TurnMoment.ToString();
        match += ";";

        match += MiniGameChosen.ToString();
        match += ";";

        int i = 0;
        foreach (var t in FilledPositions)
        {
            match += t.ToString();

            if (i < FilledPositions.Length * FilledPositions.Length)
            {
                match += ",";
            }
        }
        match += ";";

        match += NumFilled.ToString();
        match += ";";

        match += ActualChip.ToString();
        match += ";";

        match += ActualChipTeam;
        match += ";";

        return match;
    }

    /// <summary>
    /// Método para transformar una cadena de texto en una partida
    /// </summary>
    /// <param name="s">Cadena de texto a transformar</param>
    /// <returns>Clave de la partida</returns>
    public void Parse(string s)
    {
        // Variables
        int section = 0;
        string value = "";
        int x = 0;
        int y = 0;
        
        // Lectura
        for (int i = 0; i < s.Length; i++)
        {
            // Condición salida
            if (s[i].Equals(']'))
            {
                break;
            }
            
            // Condición de asignación y cambio de variable
            if (s[i].Equals(';'))
            {
                switch (section)
                {
                    // Caso 0 - MatchID
                    case 0:
                        MatchId = value;
                        break;
                    // Caso 1 - OpponentID
                    case 1:
                        OpponentId = value;
                        break;
                    // Caso 2 - PlayerOName
                    case 2:
                        PlayerOName = value;
                        break;
                    // Caso 3 - PlayerXName
                    case 3:
                        PlayerXName = value;
                        break;
                    // Caso 4 - WhosTurn
                    case 4:
                        WhosTurn = value;
                        break;
                    // Caso 5 - TurnMoment
                    case 5:
                        TurnMoment = int.Parse(value);
                        break;
                    // Caso 6 - MinigameChosen
                    case 6:
                        MiniGameChosen = int.Parse(value);
                        break;
                    // Caso 7 - FilledPositions
                    case 7:
                        break;
                    // Caso 8 - NumFilled
                    case 8:
                        NumFilled = int.Parse(value);
                        break;
                    // Caso 9 - ActualChip
                    case 9:
                        ActualChip = int.Parse(value);
                        break;
                    // Caso 10 - ActualChipTeam
                    case 10:
                        ActualChipTeam = value;
                        break;
                }

                value = "";
                section++;
                continue;
            }

            // Siguiente carácter en aquellos no necesarios
            if (s[i].Equals('[') || s[i].Equals(' '))
            {
                continue;
            }

            // Lectura para variables
            switch (section)
            {
                // Caso 0 -  MatchID
                case 0:
                // Caso 1 - OpponentID
                case 1:
                // Caso 2 - PlayerOName
                case 2:
                // Caso 3 - PlayerXName
                case 3:
                // Caso 4 - WhosTurn
                case 4:
                // Caso 5 - TurnMoment
                case 5:
                // Caso 6 - MinigameChosen
                case 6:
                // Caso 8 - NumFilled
                case 8:
                // Caso 9 - ActualChip
                case 9:
                // Caso 10 - ActualChipTeam
                case 10:
                    value += s[i];
                    break;
                // Caso 7 - FilledPositions
                case 7:
                    // Condición cambio de fila
                    if (y > 2)
                    {
                        y = 0;
                        x++;
                    }

                    // Condición cambio de valor
                    if (s[i].Equals(','))
                    {
                        FilledPositions[x, y] = int.Parse(value);

                        y++;
                        value = "";
                        continue;
                    }
                    
                    value += s[i];
                    break;
            }
        }
    }

    #endregion
}
