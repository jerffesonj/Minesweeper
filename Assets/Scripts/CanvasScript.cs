using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour 
{
    [SerializeField] private TMP_Text flags;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private GameController gameController;

    [SerializeField] private Image smiley;
    [SerializeField] private Sprite[] smileyFaces;

    public delegate void OnFlagChange(int value);
    public static event OnFlagChange onFlagUsed;

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();

        ChangeFlag();

        CellScript.onFlagChanged += ChangeFlag;
        GameController.onGameLost += DeadSmiley;
        GameController.onGameWon += WinSmiley;
    }

    private void OnDisable()
    {
        CellScript.onFlagChanged -= ChangeFlag;
        GameController.onGameLost -= DeadSmiley;
        GameController.onGameWon -= WinSmiley;
    }

    public void DeadSmiley()
    {
        SetSmileyFace(smileyFaces[3]);
    }
    public void WinSmiley()
    {
        SetSmileyFace(smileyFaces[1]);
    }
    void SetSmileyFace(Sprite sprite)
    {
        smiley.sprite = sprite;
    }

    private void Update()
    {
        timeText.text = gameController.GetCurrentTime();
    }

    public void ChangeFlag()
    {
        flags.text = gameController.FlagCount.ToString("D2");
    }
}
