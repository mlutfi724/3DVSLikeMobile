using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using MoreMountains.Tools;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // define the different state of the enum
    public enum GameState
    {
        Gameplay,
        Pause,
        LevelUp,
        GameOver
    }

    // store the current state of the game
    public GameState CurrentState;

    // Store the previous state of the game
    public GameState PreviousState;

    public bool IsGameOver = false;

    // Reference to the player game object
    public GameObject PlayerObject;

    // Flag to check if player are choosing their upgrades
    public bool IsChoosingUpgrade;

    [Header("BGM Audio")]
    public AudioClip MusicAudio;

    public AudioClip LevelUpSFX;

    [Header("Screens UI")]
    [SerializeField] private GameObject _pauseScreen;

    [SerializeField] private GameObject _resultsScreen;
    [SerializeField] private GameObject _levelUpScreen;

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
    public TextMeshProUGUI TimeSurvivedDisplay;
    public TextMeshProUGUI LevelReachedDisplay;
    public List<Image> ChosenWeaponsUI = new List<Image>(6);
    public List<Image> ChosenPassiveItemsUI = new List<Image>(6);

    [Header("Stopwatch")]
    public float TimeLimit; // Time limit in seconds

    private float _stopwatchTime; // the current time elapsed since the stopwatch started
    public TextMeshProUGUI StopwatchDisplay;

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
        PlayBGM();
    }

    private void Update()
    {
        // Define the behaviour for each state

        switch (CurrentState)
        {
            case GameState.Gameplay:
                // code for the gameplay state
                CheckForPauseAndResume();
                UpdateStopwatch();
                break;

            case GameState.Pause:
                // code for the pause state
                CheckForPauseAndResume();
                break;

            case GameState.LevelUp:
                // code for the level up state

                if (!IsChoosingUpgrade)
                {
                    IsChoosingUpgrade = true;
                    Time.timeScale = 0f; // pause the game
                    Debug.Log("Upgrades shown!");
                    _levelUpScreen.SetActive(true);
                }
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
        _levelUpScreen.SetActive(false);
    }

    public void GameOver()
    {
        TimeSurvivedDisplay.text = StopwatchDisplay.text;
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

    private void UpdateStopwatch()
    {
        _stopwatchTime += Time.deltaTime;

        UpdateStopwatchDisplay();

        if (_stopwatchTime > TimeLimit)
        {
            PlayerObject.SendMessage("PlayerDied");
        }
    }

    private void UpdateStopwatchDisplay()
    {
        // calculate the number of minutes and seconds elapsed
        int minutes = Mathf.FloorToInt(_stopwatchTime / 60);
        int seconds = Mathf.FloorToInt(_stopwatchTime % 60);

        //Update the stopwatch text to display the elapsed time
        StopwatchDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StartLevelUp()
    {
        ChangeState(GameState.LevelUp);
        PlayLevelUpSFX();
        PlayerObject.SendMessage("RemoveAndApplyUpgrades");
    }

    public void EndLevelUp()
    {
        IsChoosingUpgrade = false;
        Time.timeScale = 1;
        _levelUpScreen.SetActive(false);
        ChangeState(GameState.Gameplay);
    }

    private void PlayBGM()
    {
        MMSoundManagerSoundPlayEvent.Trigger(MusicAudio, MMSoundManager.MMSoundManagerTracks.Music, this.transform.position, persistent: true, loop: true, fade: true, volume: 0.2f);
    }

    private void PlayLevelUpSFX()
    {
        MMSoundManagerPlayOptions options;
        options = MMSoundManagerPlayOptions.Default;
        options.Loop = false;
        options.Volume = 1f;
        options.DoNotAutoRecycleIfNotDonePlaying = false;

        MMSoundManagerSoundPlayEvent.Trigger(LevelUpSFX, options);
    }
}