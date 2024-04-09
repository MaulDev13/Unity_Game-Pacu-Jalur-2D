using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [Header("Sync")]
    [SerializeField] private TextMeshProUGUI boostTxt;

    private Movement myBoat;

    private void Start()
    {
        myBoat = GetComponent<Movement>();

        myBoat.OnTriggerBoost += BoostUI;

        BoostUI();
    }

    private void OnDisable()
    {
        myBoat.OnTriggerBoost -= BoostUI;
    }

    void BoostUI()
    {
        if (myBoat.IsBoostAvaliable)
            boostTxt.text = "Boost";
        else
            boostTxt.text = "No Boost";
    }
}
