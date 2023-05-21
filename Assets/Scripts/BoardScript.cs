using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardScript : MonoBehaviour
{
    public const int ROWS = 10;
    public const int COLUMNS = 10;
    public const int MAX_BOMBS = 10;

    [SerializeField] private int numCellsOpened;

    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private RectTransform canvas;

    private static CellScript[,] matrix;
    private List<CellScript> allCellBombed = new List<CellScript>();
    private List<CellScript> allCellFlagged = new List<CellScript>();
    private List<CellScript> allCells = new List<CellScript>();

    public int NumCellsOpened { get => numCellsOpened; set => numCellsOpened = value; }
    public List<CellScript> AllCellFlagged { get => allCellFlagged; }

    public static CellScript GetCell(int x, int y) { return matrix[x, y]; }
    public static CellScript GetCell(Vector2Int position) { return matrix[position.x, position.y]; }

    void Awake()
    {
        matrix = new CellScript[ROWS, COLUMNS];
        CreateBoard();
    }

    void CreateBoard()
    {
        float xCenter = (COLUMNS / 2);
        float yCenter = (ROWS / 2);

        if (COLUMNS % 2 == 0)
        {
            xCenter -= 0.5f;
        }

        if (ROWS % 2 == 0)
        {
            yCenter -= 0.5f;
        }

        for (int i = 0; i < COLUMNS; i++)
        {
            for (int j = 0; j < ROWS; j++)
            {
                GameObject cellClone = Instantiate(cellPrefab, canvas);
                CellScript cellScript = cellClone.GetComponent<CellScript>();
                cellScript.CurrentPosition = new Vector2Int(i, j);

                matrix[i, j] = cellScript;
                allCells.Add(cellScript);
                cellClone.GetComponent<RectTransform>().anchoredPosition = new Vector2((i - yCenter) * 100, (j - xCenter) * 100);
            }
        }

        SetRandomBombs();

        RandomCellList();
    }

    void RandomCellList()
    {
        for (int i = 0; i < allCells.Count; i++)
        {
            int randomValue = Random.Range(0, allCells.Count);
            CellScript cell = allCells[i];
            CellScript otherCell = allCells[randomValue];

            allCells[i] = otherCell;
            allCells[randomValue] = cell;
        }
    }

    void SetRandomBombs()
    {
        int currentBombs = MAX_BOMBS;

        while (currentBombs > 0)
        {
            int randomXPosition = Random.Range(0, ROWS);
            int randomYPosition = Random.Range(0, COLUMNS);

            CellScript cell = GetCell(randomXPosition, randomYPosition);

            if (cell.HasBomb)
                continue;

            cell.HasBomb = true;

            currentBombs--;

            allCellBombed.Add(cell);
        }
    }

    public void RandomBomb(Vector2Int position)
    {
        CellScript currentCell = GetCell(position);
        currentCell.HasBomb = false;
        allCellBombed.Remove(currentCell);

        int randomXPosition = Random.Range(0, ROWS);
        int randomYPosition = Random.Range(0, COLUMNS);

        while (randomXPosition == position.x && randomYPosition == position.y)
        {
            randomXPosition = Random.Range(0, ROWS);
            randomYPosition = Random.Range(0, COLUMNS);

            while (GetCell(randomXPosition, randomYPosition).HasBomb)
            {
                randomXPosition = Random.Range(0, ROWS);
                randomYPosition = Random.Range(0, COLUMNS);
            }
        }
        CellScript cell = GetCell(randomXPosition, randomYPosition);
        allCellBombed.Add(cell);

        numCellsOpened += 1;
    }

    public void ShowAllFlagsWithBombs()
    {
        foreach (CellScript cell in allCellBombed)
        {
            cell.ActiveFlag();
        }
    }

    public static float TotalTimeToShowBombs = 0.5f;

    public void ShowAllBombs()
    {
        StartCoroutine(ShowBombEnum());
    }
    IEnumerator ShowBombEnum()
    {
        yield return new WaitForSeconds(0.5f);

        foreach (CellScript cell in allCellBombed)
        {
            cell.ShowBomb();
            if (!cell.IsFlagged)
            {
                yield return new WaitForSeconds(0.5f);
            }
        }

        foreach (CellScript cell in allCellFlagged)
        {
            cell.ShowBomb();
            yield return new WaitForSeconds(0.5f);
        }
    }
}

