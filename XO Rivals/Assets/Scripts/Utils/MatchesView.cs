using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MatchesView
{
    public TextMeshPro NameText;
    public TextMeshPro StatusText;

    public MatchesView(Transform rootView)
    {
        NameText = rootView.Find("MatchName").GetComponent<TextMeshPro>();
        NameText = rootView.Find("MatchStatus").GetComponent<TextMeshPro>();
    }
}
