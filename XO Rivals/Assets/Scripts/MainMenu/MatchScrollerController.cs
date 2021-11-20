using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchScrollerController : MonoBehaviour
{
    [SerializeField] public GameObject MatchPrefab;
    [SerializeField] public ScrollRect ScrollView;
    [SerializeField] public RectTransform ViewContent;
    
    private List<MatchesView> views = new List<MatchesView>();

    private void Start()
    {
        GetMatchesList();
        StartCoroutine(UpdateMatchesList());
    }
    
    private void GetMatchesList()
    {
        FetchPlayerMatches(
            FindObjectOfType<GameManager>().PlayerMatches.Count, 
            OnRecievedMatches);
    }

    private IEnumerator UpdateMatchesList()
    {
        yield return new WaitForSeconds(5);
        FetchPlayerMatches(
            FindObjectOfType<GameManager>().PlayerMatches.Count, 
            OnRecievedMatches);
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
            var instance = Instantiate(MatchPrefab, ViewContent, false);
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
        var matchesList = count < 1 ? new MatchModel[1] : new MatchModel[count];

        if (count < 1)
        {
            matchesList[count] = new MatchModel
            {
                MatchName = "No tienes partidas activas",
                MatchStatus = ""
            };
        }
        else
        {
            int i = 0;
            foreach (Match match in FindObjectOfType<GameManager>().PlayerMatches.Values)
            {
                matchesList[i] = new MatchModel
                {
                    MatchName = match.MatchId,
                    MatchStatus = match.WhosTurn
                };
                i++;
            }
        }
        
        onDone(matchesList);
    }
}
