using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerInfo : MonoBehaviour {
        
    #region Vars

    /// <summary>
    /// ID de usuario
    /// </summary>
    public string UserID = "";
    /// <summary>
    /// Nombre de usuario
    /// </summary>
    public string Name = "";
    /// <summary>
    /// ¿Está en línea el jugador?
    /// </summary>
    public bool Online = false;
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
    public int Lives;
    /// <summary>
    /// Instante de ultima vida perdida
    /// </summary>
    public DateTime LostLifeTime;

    #endregion
    

}
