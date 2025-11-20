using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [Header("Referències de UI")]
    public Text attemptsText;
    public Text timerText;
    public Text bestScoreText;
    public Button revealButton;
    public Button restartButton;
    public Button exitButton;
    public GameObject endPanel;
    public Text endText;

    [HideInInspector] public bool revealUsed = false;

    private GameManager gameManager;
    private AudioManager audioManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        audioManager = FindObjectOfType<AudioManager>();

        if (revealButton) revealButton.onClick.AddListener(OnRevealPressed);
        if (restartButton) restartButton.onClick.AddListener(OnRestartPressed);
        if (exitButton) exitButton.onClick.AddListener(OnExitPressed);

        if (endPanel) endPanel.SetActive(false);
    }

    void Update()
    {
        if (gameManager == null) return;

        if (attemptsText) attemptsText.text = $"Intents: {gameManager.GetAttempts()}";
        if (timerText) timerText.text = $"Temps: {gameManager.GetTimer():F1}s";

        float best = PlayerPrefs.GetFloat("BestScore", 0f);
        if (best > 0f)
            bestScoreText.text = $"Millor temps: {best:F1}s";
        else
            bestScoreText.text = "";
    }

    public void ShowEndPanel(float time, int attempts, bool isNewBest)
    {
        if (endPanel == null) return;
        endPanel.SetActive(true);
        endText.text = isNewBest
            ? $"🎉 Felicitats!\nNou rècord!\nTemps: {time:F1}s\nIntents: {attempts}"
            : $"Partida completada!\nTemps: {time:F1}s\nIntents: {attempts}";
    }

    public void OnRevealPressed()
    {
        if (revealUsed) return;
        revealUsed = true;
        revealButton.interactable = false;
        StartCoroutine(RevealAll());
    }

    IEnumerator RevealAll()
    {
        audioManager?.PlayReveal();
        foreach (var token in FindObjectsOfType<Token>())
        {
            if (!token.isMatched && !token.isRevealed)
                token.ShowToken();
        }
        yield return new WaitForSeconds(1f);
        foreach (var token in FindObjectsOfType<Token>())
        {
            if (!token.isMatched && token.isRevealed)
                token.HideToken();
        }
    }

    public void OnRestartPressed()
    {
        audioManager?.PlayClick();
        gameManager.RestartGame();
    }

    public void OnExitPressed()
    {
        audioManager?.PlayClick();
        gameManager.ExitToMenu();
    }
}
