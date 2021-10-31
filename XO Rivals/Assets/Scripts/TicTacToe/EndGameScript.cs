using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameScript : MonoBehaviour
{

    #region Variables

    [SerializeField] private GameObject MatchVictory;
    [SerializeField] private GameObject MatchDefeat;
    [SerializeField] private GameObject MatchDraw;
    [SerializeField] private GameObject SurrenderVictory;

    #endregion

    #region ShowGO

    public void ShowMatchVictory()
    {
        MatchVictory.SetActive(true);
        MatchDefeat.SetActive(false);
        SurrenderVictory.SetActive(false);
    }

    public void ShowMatchDefeat()
    {
        MatchVictory.SetActive(false);
        MatchDefeat.SetActive(true);
        SurrenderVictory.SetActive(false);
    }

    public void ShowSurrenderVictory()
    {
        MatchVictory.SetActive(false);
        MatchDefeat.SetActive(false);
        SurrenderVictory.SetActive(true);
    }

    #endregion
}
