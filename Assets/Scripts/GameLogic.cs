using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
    public static GameLogic instance;
    public GameUI gameUI;
    public bool gameEnd;
    public string gameDifficulty;
    public float volume;
    public bool isFullScreen;
    public int resolutionQuality;
    public float LEVEL_CHANGE_DELAY;

    public int score, scoreInc;
    public int pipeIndex;
    public int highScore;
    public int gamesPlayed;

    public string PLAYER_PREF_HIGH_SCORE;
    public string PLAYER_PREF_COINS;
    public string PLAYER_PREF_VOLUME;
    public string PLAYER_PREF_FULLSCREEN;
    public string PLAYER_PREF_RESOLUTION;
    public string PLAYER_PREF_GAMES_PLAYED;
    public string PLAYER_PREF_DIFFICULTY;

    private void Awake()
    {
        PLAYER_PREF_HIGH_SCORE = "High Score";
        PLAYER_PREF_VOLUME = "Volume";
        PLAYER_PREF_FULLSCREEN = "Full Screen";
        PLAYER_PREF_RESOLUTION = "Resolution";
        PLAYER_PREF_GAMES_PLAYED = "Games Played";
        PLAYER_PREF_DIFFICULTY = "Difficulty";

        pipeIndex = 0;
        volume = PlayerPrefs.GetFloat(PLAYER_PREF_VOLUME);
        gameEnd = false;
        score = 0;
        if (!PlayerPrefs.HasKey(PLAYER_PREF_GAMES_PLAYED))
        {
            gamesPlayed = 0;
            PlayerPrefs.SetInt(PLAYER_PREF_GAMES_PLAYED, gamesPlayed);
        }
        else gamesPlayed = PlayerPrefs.GetInt(PLAYER_PREF_GAMES_PLAYED);
        highScore = PlayerPrefs.GetInt(PLAYER_PREF_HIGH_SCORE);
        scoreInc = 2;
        if (PlayerPrefs.HasKey(PLAYER_PREF_DIFFICULTY)) gameDifficulty = PlayerPrefs.GetString(PLAYER_PREF_DIFFICULTY);
        else gameDifficulty = "M";
        if (PlayerPrefs.GetInt(PLAYER_PREF_FULLSCREEN) == 0) isFullScreen = false;
        else isFullScreen = true;
        resolutionQuality = PlayerPrefs.GetInt(PLAYER_PREF_RESOLUTION);
        LEVEL_CHANGE_DELAY = 1;
        
        
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void IncreaseScore()
    {
        if (gameUI.player.transform.position.x > gameUI.pipeHolder.GetPipes()[pipeIndex].transform.position.x)
        {
            score += scoreInc;
            pipeIndex += 2;
        }

        if (score > highScore) highScore = score;
    }

    public IEnumerator GameEnd()
    {
        GameLogic.instance.gameUI.audio.Stop();
        yield return new WaitForSeconds(2);
        PlayerPrefs.SetInt(PLAYER_PREF_HIGH_SCORE, highScore);
        PlayerPrefs.SetInt(PLAYER_PREF_GAMES_PLAYED, gamesPlayed);
        instance.score = 0;
        instance.gameEnd = false;
        instance.pipeIndex = 0;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
