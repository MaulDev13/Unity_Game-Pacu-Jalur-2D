using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeManager : MonoBehaviour
{
    [SerializeField] private string sceneName_Level1 = "Level1";
    [SerializeField] private string sceneName_Level2 = "Level2";
    [SerializeField] private string sceneName_Level3 = "Level3";

    public void OnButtonExit()
    {

    }

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
}
