using UnityEngine;
using UnityEngine.SceneManagement;
public class CambioEscenaDerrotaVictoriaOff : MonoBehaviour
{

    public SFXManager_Pistolero sounds;
    [SerializeField]
    private GameObject victory;
    [SerializeField]
    private GameObject defeat;

    private MatchAI thisMatch;

    public bool win;

    private void Start() {
        thisMatch = FindObjectOfType<MatchAI>();
    }

    public void cambiarEscena()
    {

        if (win)
        {
            Invoke("VictoryCanvas", 3f);
        }
        else
        {
            Invoke("DefeatCanvas", 3f);
        }


    }

    public void DefeatCanvas()
    {
        defeat.SetActive(true);
        Invoke("Defeat", 3f);
        //FindObjectOfType<AudioManager>().Play("Defeat");

    }

    public void VictoryCanvas()
    {
        victory.SetActive(true);
        Invoke("Victory", 3f);
        //FindObjectOfType<AudioManager>().Play("Victory");

    }

    public void Defeat()
    {
        PlayerPrefs.SetInt("minigameWin", 0);
        thisMatch.TurnMoment = 2;
        SceneManager.LoadScene("TicTac_AI");
    }

    public void Victory()
    {
        PlayerPrefs.SetInt("minigameWin", 1);
        thisMatch.TurnMoment = 2;
        SceneManager.LoadScene("TicTac_AI");
    }

}
