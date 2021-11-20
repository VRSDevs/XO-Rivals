using System;
using UnityEngine;

public class PlayerInfo : MonoBehaviour {
        
    #region Variables

    /// <summary>
    /// Nombre de usuario
    /// </summary>
    public string Name = "";
    /// <summary>
    /// ID del jugador en partida
    /// </summary>
    public int MatchId = -1;
    /// <summary>
    /// Nivel del jugador
    /// </summary>
    public float Level;
    /// <summary>
    /// Vidas del jugador
    /// </summary>
    public int Lifes;
    /// <summary>
    /// Instante de ultima vida perdida
    /// </summary>
    public DateTime LostLifeTime;

    #endregion
    

}
