using UnityEngine;
using UnityEngine.EventSystems;

public class CellScript : MonoBehaviour, IPointerClickHandler
{
    private BoardScript boardScript;
    private GameController gameController;
    private CellSprite cellSprite;
    private Vector2Int currentPosition;

    private bool hasBomb;
    private bool isOpen;
    private bool isFlagged;

    public Vector2Int CurrentPosition { get => currentPosition; set => currentPosition = value; }
    public bool HasBomb { get => hasBomb; set => hasBomb = value; }
    public CellSprite CellSprite { get => cellSprite; }
    public bool IsFlagged { get => isFlagged; }
    public bool IsOpen { get => isOpen; }

    public delegate void OnFlagUsed();
    public static event OnFlagUsed onFlagChanged;
 
    void Start()
    {
        boardScript = FindObjectOfType<BoardScript>();
        gameController = FindObjectOfType<GameController>();
        cellSprite = GetComponent<CellSprite>();
    }

    public void ShowBomb()
    {
        if (hasBomb)
        {
            if (!isFlagged)
            {
                cellSprite.BombSprite();
            }
        }
        else
        {
            if (isFlagged)
            {
                cellSprite.WrongFlag();
            }
        }
        onFlagChanged?.Invoke();
    }

    public void Flag()
    {
        if (isOpen)
            return;
        if (gameController.GameEnded)
            return;

        CheckFlagActive();
    }

    void CheckFlagActive()
    {
        if (!isFlagged)
            ActiveFlag();
        else
            DeactiveFlag();
    }

    public void ActiveFlag()
    {
        if (!isFlagged)
        {
            cellSprite.FlagSprite();
            isFlagged = true;
            gameController.FlagCount -= 1;
        }
        onFlagChanged?.Invoke();
    }

    public void DeactiveFlag()
    {
        if (isFlagged)
        {
            cellSprite.CloseSprite();
            isFlagged = false;
            gameController.FlagCount += 1;
        }
        onFlagChanged?.Invoke();
    }


    public void OpenSprite()
    {
        if (isOpen) 
            return;
        if (isFlagged)
            return;
        if (gameController.GameEnded)
            return;

        isOpen = true;

        if (hasBomb)
        {
            if (boardScript.NumCellsOpened == 0)
            {
                boardScript.RandomBomb(currentPosition);
            }
            else
            {
                ShowBomb();
                gameController.PlayerLose();
                return;
            }
        }

        ChangeValueBombsSprite();

        boardScript.NumCellsOpened += 1;
        gameController.CheckIfGameWon();

        if (gameController.GameEnded)
            return;

        VisitAdjacent();
    }

    int ChangeValueBombsSprite()
    {
        int adjacentMines = AdjacentMines(currentPosition.x, currentPosition.y);
        cellSprite.SpriteNumber(adjacentMines);

        return adjacentMines;
    }

    void VisitAdjacent()
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                    continue;
               
                Vector2Int adjacentPositon = new Vector2Int(currentPosition.x + i, currentPosition.y + j);

                if (adjacentPositon.x < 0 || adjacentPositon.x >= BoardScript.ROWS)
                    continue;

                if (adjacentPositon.y < 0 || adjacentPositon.y >= BoardScript.COLUMNS)
                    continue;

                CellScript adjacentCell = BoardScript.GetCell(adjacentPositon);

                if (adjacentCell.isOpen)
                    continue;

                if (adjacentCell.isFlagged)
                    continue;

                int numMines = ChangeValueBombsSprite();

                if (!isOpen)
                {
                    boardScript.NumCellsOpened += 1;
                    gameController.CheckIfGameWon();
                    isOpen = true;
                }

                if (numMines >= 1)
                    break;

                adjacentCell.VisitAdjacent();
            }
        }
    }


    int AdjacentMines(int x, int y)
    {
        int numMines = 0;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                    continue;

                Vector2Int adjacentPositon = new Vector2Int(x + i, y + j);


                if (adjacentPositon.x < 0 || adjacentPositon.x >= BoardScript.ROWS)
                    continue;

                if (adjacentPositon.y < 0 || adjacentPositon.y >= BoardScript.COLUMNS)
                    continue;

                CellScript adjacentCell = BoardScript.GetCell(adjacentPositon);
                
                if (adjacentCell.hasBomb)
                {
                    numMines++;
                }
            }
        }
        return numMines;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Flag();
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            OpenSprite();
        }
    }
}
