using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MatchesView
{
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI StatusText;

    public MatchesView(Transform rootView)
    {
        NameText = rootView.Find("MatchName").GetComponent<TextMeshProUGUI>();
        StatusText = rootView.Find("MatchStatus").GetComponent<TextMeshProUGUI>();
    }
}
