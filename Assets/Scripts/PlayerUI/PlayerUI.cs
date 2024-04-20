using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [Header("Sync")]
    [SerializeField] private TextMeshProUGUI boostTxt;
    [SerializeField] private TextMeshProUGUI boatPositionTxt;

    private Movement myBoat;

    private void Start()
    {
        myBoat = GetComponent<Movement>();

        if (boostTxt != null)
        {
            myBoat.OnTriggerBoost += BoostUI;

            BoostUI();
        }

        if (boatPositionTxt != null)
        {
            myBoat.OnRankingChange += RankingUI;

            RankingUI();
        }
    }

    void RankingUI()
    {
        boatPositionTxt.text = $"#{myBoat.ranking}".ToString();
    }

    private void OnDisable()
    {
        if (boostTxt != null)
            myBoat.OnTriggerBoost -= BoostUI;

        if (boatPositionTxt != null)
            myBoat.OnRankingChange -= RankingUI;
    }

    void BoostUI()
    {
        if (myBoat.IsBoostAvaliable)
            boostTxt.text = "Boost";
        else
            boostTxt.text = "No Boost";
    }
}
