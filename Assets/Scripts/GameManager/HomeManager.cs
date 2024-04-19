using UnityEngine;
using UnityEngine.SceneManagement;

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
                SceneManager.LoadScene(sceneName_Level2, LoadSceneMode.Single);
                break;
            case 3:
                SceneManager.LoadScene(sceneName_Level3, LoadSceneMode.Single);
                break;
            default:
                Debug.Log($"No level {value}!");
                break;
        }
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
