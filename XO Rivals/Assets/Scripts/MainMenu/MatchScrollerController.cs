using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchScrollerController : MonoBehaviour
{
    [SerializeField] public RectTransform MatchPrefab;
    [SerializeField] public ScrollRect ScrollView;
    [SerializeField] public RectTransform ViewContent;
    
    private List<MatchesView> views = new List<MatchesView>();

    public void UpdateMatchesList()
    {
        FetchPlayerMatches(5, OnRecievedMatches);
    }
    
    private void OnRecievedMatches(MatchModel[] list)
    {
        foreach (Transform child in ViewContent)
        {
            Destroy(child.gameObject);
        }
        
        views.Clear();

        foreach (var matchModel in list)
        {
            var instance = Instantiate(MatchPrefab.gameObject, ViewContent, false);
            var view = IntializeMatchView(instance, matchModel);
            views.Add(view);
        }
    }
    
    private MatchesView IntializeMatchView(GameObject viewGO, MatchModel model)
    {
        MatchesView view = new MatchesView(viewGO.transform)
        {
            NameText =
            {
                text = model.MatchName
            },
            StatusText =
            {
                text = model.MatchStatus
            }
        };

        return view;
    }
    
    private void FetchPlayerMatches(int count, Action<MatchModel[]> onDone)
    {
        var matchesList = new MatchModel[count];
        for (int i = 0; i < matchesList.Length; i++)
        {
            matchesList[i] = new MatchModel();
            matchesList[i].MatchName = "Sala " + i;
            matchesList[i].MatchStatus = "Turno de X";
        }

        onDone(matchesList);
    }
}
