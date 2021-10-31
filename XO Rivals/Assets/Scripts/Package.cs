using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Package 
{
    #region Variables

    public string MatchId;
    public string OwnerId;
    public PlayerInfo Player;
    public PlayerInfo WhosTurn;
    public int NumFilled;
    public int[,] FilledPositions;
    public List<GameObject> Chips;
    public int MinigameChosen;

    #endregion

    #region SerializationMethods
/*
    public static object Deserialize(byte[] data)
    {
        var pck = new Package();

        byte[] matchIdBytes = new byte[sizeof()];

        return pck;
    }
    
    */

    #endregion
}
