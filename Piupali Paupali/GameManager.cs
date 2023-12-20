using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState State;

    public static event Action<GameState> OnGameStateChanged;

    public SpawnSystem spwn;
    public GameObject playerObject;
    PlayerMovement pm;
    

    // UI elements:
    [SerializeField] Image fadePanel;
    [SerializeField] CanvasGroup mainMenuCG;
    [SerializeField] CanvasGroup runningCG;
    [SerializeField] CanvasGroup gameOverCG;
    public Slider expSlider;
    [SerializeField] TextMeshProUGUI progressText;
    [SerializeField] Image healthBar;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        playerObject = GameObject.FindGameObjectWithTag("Player");
        pm = playerObject.GetComponent<PlayerMovement>();
        UpdateGameState(GameState.MainMenu);
    }



    public void UpdateGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.MainMenu:
                HandleMainMenu();
                break;
            case GameState.Running:
                HandleRunning();
                break;
            case GameState.GameOver:
                HandleGameOver();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

    }

    private void HandleGameOver()
    {
        gameOverCG.gameObject.SetActive(true);
        runningCG.gameObject.SetActive(true);
        mainMenuCG.gameObject.SetActive(false);
        spwn.isRunning = false;
        //throw new NotImplementedException();
    }

    private void HandleRunning()
    {
        Progression.FreshRun();
        gameOverCG.gameObject.SetActive(false);
        runningCG.gameObject.SetActive(true);
        mainMenuCG.gameObject.SetActive(false);
        spwn.isRunning = true;
        spwn.StartSpawning();
        //throw new NotImplementedException();
    }

    private void HandleMainMenu()
    {
        gameOverCG.gameObject.SetActive(false);
        runningCG.gameObject.SetActive(false);
        mainMenuCG.gameObject.SetActive(true);
        //throw new NotImplementedException();
    }

    public void UpdateHealthBar(float value)
    {
        healthBar.fillAmount = value;
    }

    public void UpdateExpSlider()
    {
            Debug.Log(Progression.GetExp());
            if (Progression.GetExp() >= (Progression.nextLevelUp + 1 * Progression.GetProgressionLevel())) {
                Progression.LevelUp();
                pm.LevelUp();
                progressText.text = "Progress - Lvl. " + Progression.GetProgressionLevel();
            }

            expSlider.maxValue = Progression.nextLevelUp + 1 * Progression.GetProgressionLevel();
            expSlider.value = Progression.GetExp();
        
    }

    public void StartGame()
    {
        Instance.UpdateGameState(GameState.Running);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

}

public enum GameState
{
    MainMenu,
    Running,
    GameOver
}
