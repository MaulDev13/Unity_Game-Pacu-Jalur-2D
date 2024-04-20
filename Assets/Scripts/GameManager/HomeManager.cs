using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Local game manager yang digunakan pada scene home/beranda.
/// </summary>

public class HomeManager : MonoBehaviour
{
    [Header("Sync")]
    [Tooltip("Nama scene - level 1")]
    [SerializeField] private string sceneName_Level1 = "Level1";
    [Tooltip("Nama scene - level 2")]
    [SerializeField] private string sceneName_Level2 = "Level2";
    [Tooltip("Nama scene - level 2")]
    [SerializeField] private string sceneName_Level3 = "Level3";

    [SerializeField] private GameObject ceklisBGM;
    [SerializeField] private GameObject ceklisSFX;

    [SerializeField] private Sprite level2_locked;
    [SerializeField] private Sprite level2_unlocked;

    [SerializeField] private Sprite level3_locked;
    [SerializeField] private Sprite level3_unlocked;

    [SerializeField] private Image level2Img;
    [SerializeField] private Image level3Img;

    private bool level2;
    private bool level3;

    private void Start()
    {
        OnLevelCheck();
    }

    // Button Exit
    public void OnButtonExit()
    {
        // Tidak berfungsi ketika run di editor
        Application.Quit();
    }

    // Scene manager. Change to corresponding level scene.
    public void OnButtonLevel(int value)
    {
        switch (value)
        {
            case 1:
                SceneManager.LoadScene(sceneName_Level1, LoadSceneMode.Single);
                break;
            case 2:
                if (level2)
                    SceneManager.LoadScene(sceneName_Level2, LoadSceneMode.Single);
                break;
            case 3:
                if (level3)
                    SceneManager.LoadScene(sceneName_Level3, LoadSceneMode.Single);
                break;
            default:
                Debug.Log($"No level {value}!");
                break;
        }
    }

    public void OnLevelCheck()
    {
        // Level 1

        // Level 2
        if(PlayerPrefs.GetInt("Level2") == 1)
        {
            level2Img.sprite = level2_unlocked;
            level2 = true;
        }
        else
        {
            level2Img.sprite = level2_locked;
            level2 = false;
        }

        // Level 3
        if (PlayerPrefs.GetInt("Level3") == 1)
        {
            level3Img.sprite = level3_unlocked;
            level3 = true;
        }
        else
        {
            level3Img.sprite = level3_locked;
            level3 = false;
        }
    }

    public void OnResetLevel()
    {
        PlayerPrefs.DeleteKey("Level2");
        PlayerPrefs.DeleteKey("Level3");

        OnLevelCheck();
    }

    public void OnButtonBGM()
    {
        ceklisBGM.SetActive(!ceklisBGM.activeInHierarchy);

    }

    public void OnButtonSFX()
    {
        ceklisSFX.SetActive(!ceklisSFX.activeInHierarchy);
    }
}
