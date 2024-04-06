using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Movement> boats = new List<Movement>();

    [SerializeField] private TextMeshProUGUI countDownText;
    [SerializeField] private GameObject countDownPanel;
    [SerializeField] private GameObject homeButton;

    [SerializeField] private string homeSceneName = "Home";

    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }

    private void Start()
    {
        countDownText.text = "";
        countDownPanel.SetActive(true);
        homeButton.SetActive(false);

        Invoke("StartTheGame", 1f);
    }

    private void StartTheGame()
    {
        StartCoroutine(StartAnim());
    }

    IEnumerator StartAnim()
    {
        countDownText.text = "1";
        yield return new WaitForSeconds(1f);

        countDownText.text = "2";
        yield return new WaitForSeconds(1f);

        countDownText.text = "3";
        yield return new WaitForSeconds(1f);

        countDownText.text = "GO!";
        yield return new WaitForSeconds(1f);

        countDownText.text = "";
        countDownPanel.SetActive(false);

        foreach (Movement boat in boats)
        {
            boat.isStart = true;
        }
    }

    public void PlayerFinish()
    {
        countDownText.text = "YOU WIN!";
        countDownPanel.SetActive(true);
        homeButton.SetActive(true);
    }

    public void HomeScene()
    {
        SceneManager.LoadScene(homeSceneName, LoadSceneMode.Single);
    }
}
