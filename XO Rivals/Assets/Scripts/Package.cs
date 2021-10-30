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
}
