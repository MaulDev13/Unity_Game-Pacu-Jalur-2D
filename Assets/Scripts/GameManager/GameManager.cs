using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Local game manager yang digunakan pada scene permainan.
/// </summary>

public class GameManager : MonoBehaviour
{
    [Header("Sync")]
    [Tooltip("Banyak perahu yang ada pada scene permainan.")]
    [SerializeField] private List<Movement> boats = new List<Movement>();

    [Tooltip("Text. Berada pada bagian canvas -> countDownPanel.")]
    [SerializeField] private TextMeshProUGUI countDownText;
    [Tooltip("Panel. Berada pada bagian canvas.")]
    [SerializeField] private GameObject countDownPanel;
    [Tooltip("Panel. Berada pada bagian canvas.")]
    [SerializeField] private GameObject homeButton;

    [Tooltip("Panel. Berada pada bagian canvas.")]
    [SerializeField] private GameObject pausePanel;

    [Tooltip("Panel. Berada pada bagian canvas.")]
    [SerializeField] private GameObject pauseButton;

    [Tooltip("Nama scene home")]
    [SerializeField] private string homeSceneName = "Home";

    private bool onPause;

    #region Singleton
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }
    #endregion

    #region GameplayManager
    private void Start()
    {
        // UI Check
        countDownText.text = "";
        countDownPanel.SetActive(true);
        homeButton.SetActive(false);

        pausePanel.SetActive(false);
        pauseButton.SetActive(false);

        // Start the game on 1f
        Invoke("StartTheGame", 1f);
    }

    private void StartTheGame()
    {
        StartCoroutine(StartAnim());
    }

    IEnumerator StartAnim()
    {
        // Animation
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

        // Make the boat avaliable to move
        foreach (Movement boat in boats)
        {
            boat.isStart = true;
        }

        yield return new WaitForSeconds(1f);
        pauseButton.SetActive(true);
    }

    // Ketika player boat berhasil sampai jalur finish
    public void PlayerFinish()
    {
        countDownText.text = "YOU WIN!";
        countDownPanel.SetActive(true);
        homeButton.SetActive(true);
    }
    #endregion

    #region GameSystem
    // Scene manager. Change to home scene.
    public void HomeScene()
    {
        if (onPause)
            Unpause();

        SceneManager.LoadScene(homeSceneName, LoadSceneMode.Single);
    }

    public void Pause()
    {
        Time.timeScale = 0f;

        onPause = true;
    }

    public void Unpause()
    {
        Time.timeScale = 1f;

        onPause = false;
    }

    public void Restart()
    {
        if (onPause)
            Unpause();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }
    #endregion
}
