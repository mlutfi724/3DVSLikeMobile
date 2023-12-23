using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // define the different state of the enum
    public enum GameState
    {
        Gameplay,
        Pause,
        GameOver
    }

    // store the current state of the game
    public GameState CurrentState;

    // Store the previous state of the game
    public GameState PreviousState;

    public bool IsGameOver = false;

    [Header("Screens UI")]
    [SerializeField] private GameObject _pauseScreen;

    [SerializeField] private GameObject _resultsScreen;

    //Current stats display
    [Header("Current Stats UI")]
    public TextMeshProUGUI CurrentHealthDisplay;

    public TextMeshProUGUI CurrentRecoveryDisplay;
    public TextMeshProUGUI CurrentMoveSpeedDisplay;
    public TextMeshProUGUI CurrentMightDisplay;
    public TextMeshProUGUI CurrentProjectileSpeedDisplay;
    public TextMeshProUGUI CurrentMagnetDisplay;

    [Header("Results Screen UI")]
    public Image ChosenCharacterImage;

    public TextMeshProUGUI ChosenCharacterName;
    public TextMeshProUGUI LevelReachedDisplay;
    public List<Image> ChosenWeaponsUI = new List<Image>(6);
    public List<Image> ChosenPassiveItemsUI = new List<Image>(6);

    private void Awake()
    {
        // Warning check if there is another singleton of this kind!
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("EXTRA " + this + " DELETED!");
            Destroy(gameObject);
        }
        DisableScreens();
    }

    private void Update()
    {
        // Define the behaviour for each state

        switch (CurrentState)
        {
            case GameState.Gameplay:
                // code for the gameplay state
                CheckForPauseAndResume();
                break;

            case GameState.Pause:
                // code for the pause state
                CheckForPauseAndResume();
                break;

            case GameState.GameOver:
                // code for the gameOver state
                if (!IsGameOver)
                {
                    IsGameOver = true;
                    DisplayResults();
                    Time.timeScale = 0f;

                    Debug.Log("GAME IS OVER!");
                }
                break;

            default:
                Debug.LogWarning("State Doesn't Exist!");
                break;
        }
    }

    // method to use for changing states
    public void ChangeState(GameState newState)
    {
        CurrentState = newState;
    }

    public void PauseGame()
    {
        if (CurrentState != GameState.Pause)
        {
            PreviousState = CurrentState;
            ChangeState(GameState.Pause);
            _pauseScreen.SetActive(true);
            Time.timeScale = 0;
            Debug.Log("Game paused!");
        }
    }

    public void ResumeGame()
    {
        if (CurrentState == GameState.Pause)
        {
            ChangeState(PreviousState);
            _pauseScreen.SetActive(false);
            Time.timeScale = 1;
            Debug.Log("Game resumed!");
        }
    }

    private void CheckForPauseAndResume()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (CurrentState == GameState.Pause)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    private void DisableScreens()
    {
        _pauseScreen.SetActive(false);
        _resultsScreen.SetActive(false);
    }

    public void GameOver()
    {
        ChangeState(GameState.GameOver);
    }

    private void DisplayResults()
    {
        _resultsScreen.SetActive(true);
    }

    public void AssignChosenCharacterUI(CharacterScriptableObject chosenCharacterData)
    {
        ChosenCharacterImage.sprite = chosenCharacterData.CharacterIcon;
        ChosenCharacterName.text = chosenCharacterData.CharacterName;
    }

    public void AssignLevelReachedUI(int levelReachedData)
    {
        LevelReachedDisplay.text = levelReachedData.ToString();
    }

    public void AssignChosenWeaponAndPassiveItemsUI(List<Image> chosenWeaponsData, List<Image> chosenPassiveItemsData)
    {
        if (chosenWeaponsData.Count != ChosenWeaponsUI.Count || chosenPassiveItemsData.Count != ChosenPassiveItemsUI.Count)
        {
            Debug.Log("Chosen weapons and passive items data lists have different lenghts!");
            return;
        }

        // assign chosen weapons data to ChosenWeaponsUI
        for (int i = 0; i < ChosenWeaponsUI.Count; i++)
        {
            // check that the sprite in the corresponding element in chosenWeaponsData is not null
            if (chosenWeaponsData[i].sprite)
            {
                // enable the corresponding element in chosenWeaponsUI and set its sprite to the corresponding sprite in chosenWeaponsData[]
                ChosenWeaponsUI[i].enabled = true;
                ChosenWeaponsUI[i].sprite = chosenWeaponsData[i].sprite;
            }
            else
            {
                // If the sprite is null, disable the corresponding element in ChosenWeaponsUI
                ChosenWeaponsUI[i].enabled = false;
            }
        }

        // assign chosen passive items data to ChosenPassiveItemsUI
        for (int i = 0; i < ChosenPassiveItemsUI.Count; i++)
        {
            // check that the sprite in the corresponding element in chosenPassiveItemsData is not null
            if (chosenPassiveItemsData[i].sprite)
            {
                // enable the corresponding element in ChosenPassiveItemsUI and set its sprite to the corresponding sprite in chosenPassiveItemsData[]
                ChosenPassiveItemsUI[i].enabled = true;
                ChosenPassiveItemsUI[i].sprite = chosenPassiveItemsData[i].sprite;
            }
            else
            {
                // If the sprite is null, disable the corresponding element in ChosenPassiveItemsUI
                ChosenPassiveItemsUI[i].enabled = false;
            }
        }
    }
}