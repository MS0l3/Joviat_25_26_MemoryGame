using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject prefabToken;
    public int rows = 4;
    public int cols = 4;
    public float spacing = 2f;
    public GameObject[,] tokens;
    public Material[] materials;

    private int numTokensOpened;
    private string token1Name;
    private string token2Name;

    // --- NOVES VARIABLES ---
    private int attempts = 0;
    private float timer = 0f;
    private bool gameRunning = false;
    private int pairsFound = 0;
    private int totalPairs;

    private UIManager uiManager;
    private AudioManager audioManager;

    void Start()
    {
        rows = PlayerPrefs.GetInt("rows", rows);
        cols = PlayerPrefs.GetInt("cols", cols);
        numTokensOpened = 0;
        tokens = new GameObject[rows, cols];
        totalPairs = (rows * cols) / 2;

        uiManager = FindObjectOfType<UIManager>();
        audioManager = FindObjectOfType<AudioManager>();

        // Generar tokens
        Vector3 startPos = new Vector3(-((cols - 1) * spacing) / 2, 0, ((rows - 1) * spacing) / 2);
        int indexM = 0;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Vector3 pos = startPos + new Vector3(j * spacing * 1.25f, 0, -i * spacing);
                GameObject o = Instantiate(prefabToken, pos, Quaternion.identity);
                o.name = $"Token_{i}_{j}";
                o.GetComponent<Token>().mr.material = materials[indexM];
                indexM = (indexM + 1) % materials.Length;
                tokens[i, j] = o;
            }
        }

        audioManager?.PlayStart();
        gameRunning = true;
    }

    void Update()
    {
        if (gameRunning)
            timer += Time.deltaTime;
    }

    public void TokenPressed(string name)
    {
        if (!gameRunning) return;
        if (numTokensOpened >= 2) return;

        audioManager?.PlayClick();

        if (numTokensOpened == 0)
        {
            token1Name = name;
        }
        else if (numTokensOpened == 1)
        {
            if (token1Name == name) return;
            token2Name = name;
        }

        Token token = GetTokenByName(name);
        token.ShowToken();
        numTokensOpened++;

        if (numTokensOpened == 2)
        {
            attempts++;
            Invoke("CheckTokens", 1.2f);
            numTokensOpened = 3; // bloqueja fins comprovar
        }
    }

    private Token GetTokenByName(string name)
    {
        string[] parts = name.Split('_');
        int i = int.Parse(parts[1]);
        int j = int.Parse(parts[2]);
        return tokens[i, j].GetComponent<Token>();
    }

    public void CheckTokens()
    {
        Token t1 = GetTokenByName(token1Name);
        Token t2 = GetTokenByName(token2Name);

        if (t1.mr.material.name == t2.mr.material.name)
        {
            audioManager?.PlayMatch();
            t1.MatchToken();
            t2.MatchToken();
            pairsFound++;

            if (pairsFound == totalPairs)
                EndGame();
        }
        else
        {
            audioManager?.PlayMismatch();
            t1.HideToken();
            t2.HideToken();
        }

        numTokensOpened = 0;
    }

    private void EndGame()
    {
        gameRunning = false;
        float best = PlayerPrefs.GetFloat("BestScore", 0f);
        bool newBest = false;

        if (best == 0f || timer < best)
        {
            newBest = true;
            PlayerPrefs.SetFloat("BestScore", timer);
            PlayerPrefs.Save();
            audioManager?.PlayNewBest();
        }

        uiManager?.ShowEndPanel(timer, attempts, newBest);
    }

    // --- FUNCIONS PER AL UIManager ---
    public int GetAttempts() => attempts;
    public float GetTimer() => timer;

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitToMenu()
    {
        // Si tens una escena de menú principal anomenada "MainMenu"
        // Si no, pots posar el nom correcte o deixar-ho buid.
        SceneManager.LoadScene("MainMenu");
    }
}
