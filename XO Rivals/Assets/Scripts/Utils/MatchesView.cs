using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MatchesView
{
    #region Vars

    /// <summary>
    /// Referencia al componente TextMeshProUGUI de la ID de la partida
    /// </summary>
    public TextMeshProUGUI IDText;
    /// <summary>
    /// Referencia al componente TextMeshProUGUI del nombre de la partida
    /// </summary>
    public TextMeshProUGUI NameText;
    /// <summary>
    /// Referencia al componente TextMeshProUGUI del estado de la partida (turno)
    /// </summary>
    public TextMeshProUGUI StatusText;

    #endregion

    #region Constructors

    public MatchesView(Transform rootView)
    {
        IDText = rootView.Find("MatchID").GetComponent<TextMeshProUGUI>();
        NameText = rootView.Find("MatchName").GetComponent<TextMeshProUGUI>();
        StatusText = rootView.Find("MatchStatus").GetComponent<TextMeshProUGUI>();
    }

    #endregion
}
