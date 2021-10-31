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
    //public List<GameObject> Chips;
    public int MinigameChosen;

    #endregion

    #region SerializationMethods

    /*
    public object Deserialize(byte[] data)
    {
        var pck = new Package();

        int[] sizes =
        {
            sizeof(char) * MatchId.Length,
            sizeof(char) * OwnerId.Length,
            sizeof(char) * Player.Name.Length,
            sizeof(char) * WhosTurn.Name.Length,
            sizeof(int),
            sizeof(int) * FilledPositions.Length,
            sizeof(int)
        };

        byte[] matchIdBytes = new byte[sizes[0]];

        return pck;
        
    }
    */

    #endregion
}
