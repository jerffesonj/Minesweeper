using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private int flagCount;
    private float gameTime;
    private bool gameEnded;

    public AudioSource audioSource;
    public AudioClip win;
    public AudioClip lose;

    public delegate void OnEndGame();
    public static event OnEndGame onGameWon;
    public static event OnEndGame onGameLost;

    private BoardScript boardScript;

    public bool GameEnded { get => gameEnded; set => gameEnded = value; }
    public int FlagCount { get => flagCount; set => flagCount = value; }

    private void Awake()
    {
        boardScript = GetComponent<BoardScript>();
        audioSource = GameObject.FindGameObjectWithTag("AudioSource").GetComponent<AudioSource>();

        flagCount = BoardScript.MAX_BOMBS;
    }
    
    private void Update()
    {
        if (!gameEnded)
            gameTime += Time.deltaTime;
    }

    public void WinSFX()
    {
        audioSource.PlayOneShot(win);
    }
    public void LoseSFX()
    {
        audioSource.PlayOneShot(lose);
    }

    public string GetCurrentTime()
    {
        return TimeInMin(gameTime).ToString("D2") + ":" + TimeInSec(gameTime).ToString("D2");
    }
    public int TimeInSec(float value)
    {
        float valueInSec = value;
        valueInSec = (valueInSec) - (float)TimeInMin(value) * 60;
        return (int)valueInSec;
    }

    public int TimeInMin(float value)
    {
        float valueInMin = value;
        if (value >= 60)
        {
            valueInMin /= 60;
        }
        else
        {
            valueInMin = 0;
        }
        return (int)valueInMin;
    }

    public void CheckIfGameWon()
    {
        if (boardScript.NumCellsOpened == BoardScript.ROWS * BoardScript.COLUMNS - BoardScript.MAX_BOMBS)
        {
            PlayerWon();
        }
    }

    public void PlayerWon()
    {
        WinSFX();

        gameEnded = true;

        boardScript.ShowAllFlagsWithBombs();
        onGameWon?.Invoke();
    }

    public void PlayerLose()
    {
        LoseSFX();

        gameEnded = true;

        boardScript.ShowAllBombs();
        onGameLost?.Invoke();
    }
    
    public void ResetGame()
    {
        SceneManager.LoadScene(0);
    }
}
