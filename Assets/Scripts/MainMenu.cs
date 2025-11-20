using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayEasy()
    {
        PlayerPrefs.SetInt("rows", 3);
        PlayerPrefs.SetInt("cols", 2);
        PlayerPrefs.Save();
        LoadGame();
    }

    public void PlayMedium()
    {
        PlayerPrefs.SetInt("rows", 4);
        PlayerPrefs.SetInt("cols", 2);
        PlayerPrefs.Save();
        LoadGame();
    }

    public void PlayHard()
    {
        PlayerPrefs.SetInt("rows", 4);
        PlayerPrefs.SetInt("cols", 3);
        PlayerPrefs.Save();
        LoadGame();
    }

    private void LoadGame()
    {
        SceneManager.LoadScene("GameScene"); 
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}