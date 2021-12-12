using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchScrollerController : MonoBehaviour
{
    #region Vars

    /// <summary>
    /// Referencia al prefab de la partida
    /// </summary>
    [SerializeField] public GameObject MatchPrefab;
    /// <summary>
    /// Referencia a la vista de scroll
    /// </summary>
    [SerializeField] public ScrollRect ScrollView;
    /// <summary>
    /// Referencia a la vista del contenido
    /// </summary>
    [SerializeField] public RectTransform ViewContent;
    
    /// <summary>
    /// Lista de las vistas a insertar en el scroll
    /// </summary>
    private List<MatchesView> views = new List<MatchesView>();

    /// <summary>
    /// Número total de partidas
    /// </summary>
    private int _totalMatches;

    #endregion

    #region UnityCB

    private void Start()
    {
        _totalMatches = 0;
        
        GetMatchesList();
    }

    #endregion

    #region Algorithm

    /// <summary>
    /// Método para obtener la lista de partidas en un comienzo
    /// </summary>
    private void GetMatchesList()
    {
        _totalMatches = FindObjectOfType<GameManager>().PlayerMatches.Count;
        
        FetchPlayerMatches(OnRecievedMatches);
    }

    /// <summary>
    /// Método ejecutado al recibir la lista de partidas del diccionario
    /// </summary>
    /// <param name="list">Lista de partidas</param>
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
            if (_totalMatches < 1) instance.GetComponent<Button>().interactable = false;
            
            var view = IntializeMatchView(instance, matchModel);
            views.Add(view);
        }
    
    }
    
    /// <summary>
    /// Método para inicializar cada prefab a introducir a la vista
    /// </summary>
    /// <param name="viewGO"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    private MatchesView IntializeMatchView(GameObject viewGO, MatchModel model)
    {
        MatchesView view = new MatchesView(viewGO.transform)
        {
            IDText =
            {
                text = model.MatchId
            },
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
    
    /// <summary>
    /// Método para obtener la lista de partidas del diccionario
    /// </summary>
    /// <param name="onDone"></param>
    private void FetchPlayerMatches(Action<MatchModel[]> onDone)
    {
        var matchesList = _totalMatches < 1 ? new MatchModel[1] : new MatchModel[_totalMatches];

        if (_totalMatches < 1)
        {
            matchesList[_totalMatches] = new MatchModel
            {
                MatchId = "",
                MatchName = "You don´t have active matches!",
                MatchStatus = ""
            };
        }
        else
        {
            int i = 0;
            foreach (Match match in FindObjectOfType<GameManager>().PlayerMatches.Values)
            {
                string opponent = match.PlayerOName.Equals(FindObjectOfType<PlayerInfo>().Name)
                    ? match.PlayerXName
                    : match.PlayerOName;
                matchesList[i] = new MatchModel
                {
                    MatchId = match.MatchId,
                    MatchName = "Match against " + opponent,
                    MatchStatus = "Original"
                };
                i++;
            }
        }
        
        onDone(matchesList);
    }

    #endregion
}
