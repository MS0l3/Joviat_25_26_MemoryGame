using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DifficultyButton : MonoBehaviour
{
    public TextMeshProUGUI difficultyText;   // Text dins el botó de difficulty
    public TextMeshProUGUI startButtonText;  // (Opcional) Text del botó Start per mostrar la mida

    private string[] difficulties = { "Easy", "Medium", "Hard" };
    private int currentIndex = 0;

    private void Start()
    {
        // Carregar la dificultat guardada (nom)
        string saved = PlayerPrefs.GetString("difficulty", "Easy");
        currentIndex = System.Array.IndexOf(difficulties, saved);
        if (currentIndex < 0) currentIndex = 0;

        // També assegurem que rows/cols estiguin sincronitzats al iniciar
        ApplyRowsColsForIndex(currentIndex, save: false);
        UpdateUI();
        Debug.Log($"[DifficultyButton.Start] loaded difficulty={difficulties[currentIndex]} index={currentIndex}");
    }

    public void OnDifficultyPressed()
    {
        currentIndex = (currentIndex + 1) % difficulties.Length;
        PlayerPrefs.SetString("difficulty", difficulties[currentIndex]);
        // quan canvies la dificultat, guardem també les mesures corresponents
        ApplyRowsColsForIndex(currentIndex, save: true);
        UpdateUI();
        Debug.Log($"[DifficultyButton.OnDifficultyPressed] difficulty={difficulties[currentIndex]} rows={PlayerPrefs.GetInt("rows")} cols={PlayerPrefs.GetInt("cols")}");
    }

    private void UpdateUI()
    {
        if (difficultyText != null) difficultyText.text = "Difficulty: " + difficulties[currentIndex];

        // Opcional: mostrar la mida al Start button
        if (startButtonText != null)
        {
            int r = PlayerPrefs.GetInt("rows", 0);
            int c = PlayerPrefs.GetInt("cols", 0);
            startButtonText.text = $"Start ({r}x{c})";
        }
    }

    public void PlayGame()
    {
        // Assegurar valors finals abans de canviar d'escena
        ApplyRowsColsForIndex(currentIndex, save: true);
        Debug.Log($"[DifficultyButton.PlayGame] Loading GameScene with rows={PlayerPrefs.GetInt("rows")} cols={PlayerPrefs.GetInt("cols")}");
        SceneManager.LoadScene("GameScene");
    }

    private void ApplyRowsColsForIndex(int idx, bool save)
    {
        int r = 3, c = 2; // default Easy
        switch (idx)
        {
            case 0: // Easy 3x2
                r = 3; c = 2;
                break;
            case 1: // Medium 4x2
                r = 4; c = 2;
                break;
            case 2: // Hard 4x3
                r = 4; c = 3;
                break;
        }

        if (save)
        {
            PlayerPrefs.SetInt("rows", r);
            PlayerPrefs.SetInt("cols", c);
            PlayerPrefs.SetString("difficulty", difficulties[idx]);
            PlayerPrefs.Save();
        }
        else
        {
            // només per a UI: actualitza temporàriament sense forçar Save
            PlayerPrefs.SetInt("rows", r);
            PlayerPrefs.SetInt("cols", c);
        }
    }
}
