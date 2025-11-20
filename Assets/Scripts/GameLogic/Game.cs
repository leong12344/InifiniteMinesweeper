using System.Collections;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class Game : MonoBehaviour
{
    public static Game Instance;
    [Header("Stage Settings")]
    public int currentLevel = 1;
    public int maxLevel = 30;

    private int width;
    private int height;
    private int mineCount;
    public int maxHealth = 3;

    private Board board;
    private CellGrid grid;
    private bool gameover;
    private bool generated;
    private int revealCountThisClick = 0;
    private int currentHealth;

    private void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
        board = GetComponentInChildren<Board>();
    }

    private void Start()
    {
        StartLevel(currentLevel);
    }

    private void Update()
    {
        if (PauseMenu.Instance != null && PauseMenu.Instance.isPaused)
        {
            return;
        }

        if(BetweenLevelsUI.Instance != null && BetweenLevelsUI.Instance.panel.activeSelf)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.N) || Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Space))
        {
            RestartLevel();
            return;
        }

        if (!gameover)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Reveal();
                AudioManager.Instance.PlayClick();
            }

            else if (Input.GetMouseButtonDown(1)) Flag();
            else if (Input.GetMouseButton(2)) Chord();
            else if (Input.GetMouseButtonUp(2)) Unchord();
        }
    }


    private void StartLevel(int level)
    {
        currentLevel = level;

        if (level <= 5)
        {
            width = height = 10;
        }
        else if (level <= 10)
        {
            width = height = 15;
        }
        else if (level <= 15)
        {
            width = height = 20;
        }
        else if (level <= 20)
        {
            width = height = 25;
        }
        else
        {
            width = height = 30;
        }

        AudioManager.Instance.PlayMusicForLevel(level);


        float basePercent = 0.10f;           
        float growthPerLevel = 0.015f;         
        float acceleratedGrowth = Mathf.Max(0, level - 5) * 0.01f; 

        float minePercent = basePercent + level * growthPerLevel + acceleratedGrowth;

        mineCount = Mathf.RoundToInt(width * height * minePercent);

        mineCount = Mathf.Clamp(mineCount, 5, width * height - 5);


        Debug.Log($"Starting Level {level}: {width}x{height}, {mineCount} mines");
        NewGame();
    }

    public void NextLevel()
    {
        if (currentLevel < maxLevel)
        {
            currentLevel++;
            StartLevel(currentLevel);
        }
        else
        {
            Debug.Log("🎉 All levels complete!");
        }
    }

    private void RestartLevel()
    {
        Debug.Log($"🔁 Restarting Level {currentLevel}");
        StartLevel(currentLevel);
    }


    private void NewGame()
    {
        StopAllCoroutines();

        Camera.main.transform.position = new Vector3(width / 2f, height / 2f, -10f);

        gameover = false;
        generated = false;
        HeartUI.Instance.SetHearts(PlayerHealth.Instance.currentHealth);


        grid = new CellGrid(width, height);
        board.Draw(grid);
    }

    private void Reveal()
    {
        if (TryGetCellAtMousePosition(out Cell cell))
        {
            if (!generated)
            {
                grid.GenerateMines(cell, mineCount);
                grid.GenerateNumbers();
                generated = true;
            }

            revealCountThisClick = 0;

            Reveal(cell);

            ApplyRevealRewards(); 
        }
    }

    private void Reveal(Cell cell)
    {
        if (cell.revealed || cell.flagged) return;

        revealCountThisClick++;   // <-- count every revealed tile

        switch (cell.type)
        {
            case Cell.Type.Mine:
                AudioManager.Instance.PlayExplosion();
                Explode(cell);
                break;

            case Cell.Type.Empty:
                AudioManager.Instance.PlayReveal();
                StartCoroutine(Flood(cell));
                CheckWinCondition();
                break;

            default:
                AudioManager.Instance.PlayReveal();
                cell.revealed = true;
                CheckWinCondition();
                break;
        }

        board.Draw(grid);
    }


    private IEnumerator Flood(Cell cell)
    {
        if (gameover || cell.type == Cell.Type.Mine) yield break;

        if (!cell.revealed)
        {
            revealCountThisClick++;
        }

        if (cell.revealed) yield break;

        cell.revealed = true;
        board.Draw(grid);

        yield return null;

        if (cell.type == Cell.Type.Empty)
        {
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0) continue;
                    if (grid.TryGetCell(cell.position.x + dx, cell.position.y + dy, out Cell neighbor))
                    {
                        StartCoroutine(Flood(neighbor));
                    }
                }
            }
        }
    }


    private void Flag()
    {
        if (!TryGetCellAtMousePosition(out Cell cell)) return;
        if (cell.revealed) return;

        cell.flagged = !cell.flagged;
        board.Draw(grid);
    }

    private void Chord()
    {
        // unchord previous cells
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                grid[x, y].chorded = false;

        // chord new cells
        if (TryGetCellAtMousePosition(out Cell chord))
        {
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    int x = chord.position.x + dx;
                    int y = chord.position.y + dy;
                    if (grid.TryGetCell(x, y, out Cell cell))
                        cell.chorded = !cell.revealed && !cell.flagged;
                }
            }
        }

        board.Draw(grid);
    }

    private void Unchord()
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                if (grid[x, y].chorded)
                    Unchord(grid[x, y]);

        board.Draw(grid);
    }

    private void Unchord(Cell chord)
    {
        chord.chorded = false;

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;

                int x = chord.position.x + dx;
                int y = chord.position.y + dy;

                if (grid.TryGetCell(x, y, out Cell cell))
                {
                    if (cell.revealed && cell.type == Cell.Type.Number)
                    {
                        if (grid.CountAdjacentFlags(cell) >= cell.number)
                        {
                            Reveal(chord);
                            return;
                        }
                    }
                }
            }
        }
    }

    private void Explode(Cell cell)
    {
        cell.exploded = true;
        cell.revealed = true;
        board.Draw(grid);

        PlayerHealth.Instance.TakeDamage();

        if(PlayerHealth.Instance.currentHealth > 0)
        {
            return;
        }
        if(PlayerHealth.Instance.currentHealth <= 0)
        {
            gameover = true;
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cell c = grid[x, y];
                if (c.type == Cell.Type.Mine)
                    c.revealed = true;
            }
        }
        board.Draw(grid);
    }

    private void CheckWinCondition()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cell cell = grid[x, y];

                if (cell.type != Cell.Type.Mine && !cell.revealed)
                    return;
            }
        }

        gameover = true;

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                if (grid[x, y].type == Cell.Type.Mine)
                    grid[x, y].flagged = true;

        GameStats.Instance.AddLevelScore();
        GameStats.Instance.LevelUp();
        BetweenLevelsUI.Instance.ShowItemSelection();
    }

    private bool TryGetCellAtMousePosition(out Cell cell)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = board.tilemap.WorldToCell(worldPosition);
        return grid.TryGetCell(cellPosition.x, cellPosition.y, out cell);
    }

    private void ApplyRevealRewards()
    {
        if (revealCountThisClick <= 0) return;

        GameStats.Instance.AddGold(revealCountThisClick);

        if (revealCountThisClick == 1)
            GameStats.Instance.AddScore(2);
        else
            GameStats.Instance.AddScore(revealCountThisClick * 5);
    }

    public void RevealRandomMine()
    {
        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                if (grid[x, y].type == Cell.Type.Mine && !grid[x, y].revealed)
                {
                    grid[x, y].flagged = true;
                    board.Draw(grid);
                    return;
                }
    }

}
