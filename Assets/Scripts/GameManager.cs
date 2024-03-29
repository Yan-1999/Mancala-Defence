﻿/// <summary>
/// The manager class of the game.
/// Any other class MUST interact 
/// with this class instead of
/// directly interact with other game
/// classes.
/// Author: MjuTeX
/// Project: Mancala Defence
/// File: GameManager.cs
/// </summary>

using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    //singleton
    static public GameManager Instance { get; private set; } = null;

    public float TimeRate { get; set; } = 1.0f;
    public Map Map;
    public Button pauseButton;
    private bool isPause = false;
    public Text coinText;
    public Text presentLifeText;
    public Text lifeLimitText;
    public Text presentWaveText;
    public Text waveLimitText;
    public Button spawnButton;
    public Button mancalaButton;
    public Button unitUpgradeButton;
    public Button extendHandLimitButton;
    public EnemySpawner enemySpawner;
    public Canvas pauseCanvas;
    public Button continueButton;
    public Button backMenuButton;
    public Button yesButton;
    public Button noButton;
    public Text scoreText;

    public Vector3 cameraPosition;

    private List<Unit> Units { get; set; } = new List<Unit>();
    private List<Enemy> Enemies { get; set; } = new List<Enemy>();
    public PlayerAssets Assets = new PlayerAssets();
    private bool FreeMancala = false;

    public Vector3[] presetPosition;

    public int PlayerHpLimit = 10;
    public int PlayerHp = 10;
    public int Score = -150;
    

    private System.Random random = new System.Random();


    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetGamePause(false);
        DrawCard(); DrawCard(); DrawCard();
        GameObject[] gameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        coinText.text = Assets.coin.ToString("0000");
        presentLifeText.text = PlayerHp.ToString("00") + "/";
        lifeLimitText.text = PlayerHpLimit.ToString("00");
        presentWaveText.text = "01" + "/";
        waveLimitText.text = enemySpawner.waves.Length.ToString("00");
        pauseCanvas.gameObject.SetActive(false);
        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
        pauseButton.onClick.AddListener(delegate ()
        {
            if (PlayerInterface.Instance.IsActing())
            {
                return;
            }
            SetGamePause(true);
            pauseCanvas.gameObject.SetActive(true);
        });
        continueButton.onClick.AddListener(delegate ()
        {
            SetGamePause(false);
            yesButton.gameObject.SetActive(false);
            noButton.gameObject.SetActive(false);
            pauseCanvas.gameObject.SetActive(false);
        });
        backMenuButton.onClick.AddListener(delegate ()
        {
            yesButton.gameObject.SetActive(true);
            noButton.gameObject.SetActive(true);
        });
        yesButton.onClick.AddListener(delegate ()
        {
            SetGamePause(false);
            SceneManager.LoadScene("Start");
        });
        noButton.onClick.AddListener(delegate ()
        {
            yesButton.gameObject.SetActive(false);
            noButton.gameObject.SetActive(false);
        });
        spawnButton.onClick.AddListener(delegate ()
        {
            if (PlayerInterface.Instance.IsActing())
            {
                return;
            }
            PlayerCardOption(PlayerOption.UnitSpawn);
        });
        mancalaButton.onClick.AddListener(delegate ()
        {
            if (PlayerInterface.Instance.IsActing())
            {
                return;
            }
            PlayerCardOption(PlayerOption.Mancala);
        });
        unitUpgradeButton.onClick.AddListener(delegate ()
        {
            if (PlayerInterface.Instance.IsActing())
            {
                return;
            }
            PlayerCardOption(PlayerOption.UnitUpgrade);
        });
        extendHandLimitButton.onClick.AddListener(delegate ()
        {
            if (PlayerInterface.Instance.IsActing())
            {
                return;
            }
            if (Assets.CostCoin(PlayerOption.ExtendHandLimit))
            {
                Assets.HandLimit++;
                extendHandLimitButton.transform.position += new Vector3(0, Screen.height / 12);
                if (Assets.HandLimit == 9)
                {
                    extendHandLimitButton.gameObject.SetActive(false);
                }
            }
            else
            {
                PlayerInterface.Instance.ShowNoEnoughResource(false);
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        //codes below should be preserved
        if (Input.GetMouseButtonDown(0) && !PlayerInterface.Instance.IsActing())
        {
            PlayerInterface.Instance.ChooseUnit();
        }
        int count = Assets.GetMaxCount();
        if (count >= Assets.Costs[(int)PlayerOption.UnitSpawn])
        {
            spawnButton.image.sprite = Resources.Load<Sprite>("spawn");
        }
        else
        {
            spawnButton.image.sprite = Resources.Load<Sprite>("spawn-dis");
        }
        if (FreeMancala)
        {
            mancalaButton.image.sprite = Resources.Load<Sprite>("mancala-dis");
            mancalaButton.image.color = Color.yellow;
        }
        else if (count >= Assets.Costs[(int)PlayerOption.Mancala] && Units.Count > 0)
        {
            mancalaButton.image.sprite = Resources.Load<Sprite>("mancala");
        }
        else
        {
            mancalaButton.image.sprite = Resources.Load<Sprite>("mancala-dis");
        }
        if (count >= Assets.Costs[(int)PlayerOption.UnitUpgrade])
        {
            unitUpgradeButton.image.sprite = Resources.Load<Sprite>("upgrade");

        }
        else
        {
            unitUpgradeButton.image.sprite = Resources.Load<Sprite>("upgrade-dis");
        }
    }

    private void SetGamePause(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 0.0f;
            isPause = true;
        }
        else
        {
            Time.timeScale = TimeRate;
            isPause = false;
        }
    }

    /*unit placement*/
    /// <summary>
    /// Move unit from cell to cell.
    /// </summary>
    /// <param name="unit">unit to move</param>
    /// <param name="to">cell move to </param>
    private void UnitMove(Unit unit, Cell to)
    {
        Cell from = unit.OnCell;
        from.RemoveUnit(unit);
        unit.transform.SetParent(to.transform);
        unit.transform.position = to.transform.position + presetPosition[to.UnitCount() % 9];
        to.AddUnit(unit);
        unit.OnCell = to;
    }

    /// <summary>
    /// Spawn a unit on cell.
    /// This will check whether the cell is base
    /// or inactivate vul..
    /// </summary>
    /// <param name="unit">unit to spawn</param>
    /// <param name="cell">cell to place on</param>
    /// <returns>whether the spawn is successful</returns>
    public bool UnitSpawn(Unit unit, Cell cell)
    {
        if (!cell.IsVaildForUnitSpawn())
        {
            return false;
        }//can be deleted
        Unit newUnit = Instantiate(unit, cell.transform.position + presetPosition[cell.UnitCount() % 9], unit.transform.rotation);
        newUnit.LifeBar = newUnit.gameObject.GetComponentInChildren<Slider>();
        newUnit.UnitType = unit.UnitType;
        newUnit.Life = UnitFactory.Instance.Attrs[(int)newUnit.UnitType].Life;
        newUnit.Damage = UnitFactory.Instance.Attrs[(int)newUnit.UnitType].Damage;
        newUnit.Skill = UnitFactory.Instance.Attrs[(int)newUnit.UnitType].Skill;
        newUnit.LifeLimit = newUnit.Life;
        newUnit.gameObject.SetActive(true);
        newUnit.transform.SetParent(cell.transform);
        newUnit.name = "unit";
        cell.AddUnit(newUnit);
        newUnit.OnCell = cell;
        Units.Add(newUnit);
        return true;
    }


    /// <summary>
    /// Mancala operation
    /// </summary>
    /// <param name="cell"></param>
    public void Mancala(Cell cell)
    {
        int cellIndex = Array.IndexOf(Map.Cells, cell);
        Unit[] units = cell.UnitArrayCopy();
        cell.ClearUnits();
        foreach (Unit unit in units)
        {
            cellIndex = (cellIndex + 1) % Map.Cells.Length;
            if (unit == units[units.Length - 1] && 
                Map.Cells[cellIndex].GetType().Equals(typeof(BaseCell)))
            {
                FreeMancala = true;
            }
            UnitMove(unit, Map.Cells[cellIndex]);
        }
    }

    /*attack*/

    /// <returns>Returns true if cell has more than one unit
    /// and all the units on the cell is of the same type.</returns>
    public bool SameTypeUnitsOnCell(Cell cell)
    {
        if (cell.UnitCount() <= 1)
        {
            return false;
        }
        List<Unit> units = cell.UnitsOnCell();
        Unit.Type type = units[0].UnitType;
        foreach (Unit unit in units)
        {
            if (unit.UnitType != type)
            {
                return false;
            }
        }
        return true;
    }

    public void UnitAttack(Unit unit)
    {
        /*Debug.Log("122222112");
        // check if the cell is exhausted
        if (unit.OnCell.IsExhausted)
        {
            return;
        }
        List<Enemy> enemies = GameAI.FindEnemiesToAttack(unit);
        float multiplyer = (unit.OnCell.IsVulnerable) ?
            Cell.vulAttackMultiplyer : 1.0f;
        foreach (Enemy enemy in enemies)
        {
            enemy.ReceiveDamage(unit.Damage * multiplyer);
        }*/
    }
    public void EnemyAttack(Cell cell)
    {
        List<Unit> us = cell.UnitsOnCell();
        if (us.Count == 0)
        {
            return;
        }
        int[] diearray = new int[50];
        int length = 0;
        for (int i = 0; i < us.Count; i++)
        {
            if (us[i].Damage >= us[i].Life)
            {
                diearray[length] = i;
                length=1;
            }
            else
            {
                us[i].ReceiveDamage(us[i].Damage);
                us[i].LifeBar.value = us[i].Life / us[i].LifeLimit;
                if (us[i] == PlayerInterface.Instance.UnitChosen)
                {
                    PlayerInterface.Instance.upgradeLife.GetComponentInChildren<Text>().text = "Life:" +
                        PlayerInterface.Instance.UnitChosen.Life.ToString("F1") + '/' + PlayerInterface.Instance.UnitChosen.LifeLimit.ToString() + '+';
                }
            }
        }
        for (int j=0;j< length;j++)
        {
            us[diearray[j]].ReceiveDamage(us[diearray[j]].Damage);
        }
        length = 0;

        /*foreach (Unit unit in us)
        {
            unit.ReceiveDamage(unit.Damage);
            if (unit != null)
            {
                unit.LifeBar.value = unit.Life / unit.LifeLimit;
            }
            if(unit == PlayerInterface.Instance.UnitChosen)
            {
                PlayerInterface.Instance.upgradeLife.GetComponentInChildren<Text>().text = "Life:" +
                    PlayerInterface.Instance.UnitChosen.Life.ToString("F1") + '/' + PlayerInterface.Instance.UnitChosen.LifeLimit.ToString() + '+';
            }
        }*/
    }

    /// <summary>
    /// Deal with things outside <c>Unit</c>
    /// when unit dies.
    /// </summary>
    /// <param name="unit">unit to remove</param>
    /// <param name="cell"></param>
    public void UnitDeath(Unit unit)
    {
        //TODO:add some operation to change other units' position
        if (unit == PlayerInterface.Instance.UnitChosen)
        {
            PlayerInterface.Instance.CancelUpgrade();
        }
        Units.Remove(unit);
        unit.OnCell.RemoveUnit(unit);
        unit.OnCell.ChangeUnitsPosition();
    }

    public void EnemyDeath(Enemy enemy)
    {
        Enemies.Remove(enemy);
        //UNDONE:score
        IncreaseScore(500);
        Assets.GainCoinOnEmemyKill();
    }

    /*option*/
    public void PlayerCardOption(PlayerOption option)
    {
        Assert.IsTrue(option < PlayerOption.ExtendHandLimit);
        List<Unit.Type> types = Assets.CheckHand(option);
        if (FreeMancala && option == PlayerOption.Mancala)
        {
            SetGamePause(true);
            PlayerInterface.Instance.HighLightCells(option, Unit.Type.White);
            return;
        }
        if (types.Count == 0)
        {
            PlayerInterface.Instance.ShowNoEnoughResource(true);
            return;
        }
        // let player choose if there are multiple choices
        if (types.Count > 1 || option == PlayerOption.UnitUpgrade)
        {
            SetGamePause(true);
            PlayerInterface.Instance.ChooseType(option, types);
        }
        else
        {
            PlayerCardTypeCallback(option, types[0]);
        }
    }

    public void PlayerCardTypeCallback(
        PlayerOption option,
        Unit.Type type)
    {
        Assert.IsTrue(option < PlayerOption.ExtendHandLimit);
        SetGamePause(false);
        switch (option)
        {
            case PlayerOption.UnitUpgrade:
                Assets.CostCard(type, option);
                UnitFactory.Instance.UpgradeUnitFactory(type);
                //UNDONE:score
                IncreaseScore(300);
                break;
            case PlayerOption.UnitSpawn:
                SetGamePause(true);
                PlayerInterface.Instance.HighLightCells(option, type);
                break;
            case PlayerOption.Mancala:
                SetGamePause(true);
                PlayerInterface.Instance.HighLightCells(option, type);
                break;
            default:
                break;
        }
    }

    public void PlayerChooseCellCallback(
        PlayerOption option,
        Unit.Type type,
        Cell cell)
    {
        Assert.IsTrue(option == PlayerOption.UnitSpawn ||
            option == PlayerOption.Mancala);
        SetGamePause(false);
        if (FreeMancala && option == PlayerOption.Mancala)
        {
            mancalaButton.image.color = Color.white;
            FreeMancala = false;
        }
        else
        {
            Assets.CostCard(type, option);
        }
        switch (option)
        {
            case PlayerOption.UnitSpawn:
                Unit unit = UnitFactory.Instance.GenerateUnit(type);
                UnitSpawn(unit, cell);
                //UNDONE:score
                IncreaseScore(200);
                break;
            case PlayerOption.Mancala:
                Mancala(cell);
                //UNDONE:score
                IncreaseScore(300);
                break;
            default:
                break;
        }
    }


    public void PlayerCoinOption(
        PlayerOption option)
    {
        Assert.IsTrue(option > PlayerOption.Mancala);
        Assets.CostCoin(option);
        switch (option)
        {
            case PlayerOption.ExtendHandLimit:
                Assets.HandLimit++;
                break;
            //UNDONE::rewrite
            case PlayerOption.UpgradeAttribute:
                break;
            default:
                break;
        }
    }

    public void PlayerChooseUnitCallback(Unit unit, Unit.AttrEnum attrEnum)
    {
        unit.Upgrade(attrEnum);
    }

    /*game process*/
    public void DrawCard()
    {
        //UNDONE:score
        IncreaseScore(50);
        Assets.DrawCard(
            (Unit.Type)random.Next(0, Enum.GetValues(typeof(Unit.Type)).Length)
            );
    }


    public void EnemyPass()
    {
        PlayerHp--;
        presentLifeText.text = PlayerHp.ToString("00") + "/";
        //UNDONE:score
        IncreaseScore(-10000);
        if (PlayerHp == 0)
            Failed();
    }

    public void Win()
    {
        //endUI.SetActive(true);
        //endMessage.text = "胜 利";
    }

    public void Failed()
    {
        //enemySpawner.Stop();

        GameObject.Find("PlayerInterface").SendMessage("Failed");
        enemySpawner.GetComponent<EnemySpawner>().Stop();
        if(GameObject.Find("EnemySpawner1"))
            GameObject.Find("EnemySpawner1").SendMessage("Stop");
        //endUI.SetActive(true);
        //endMessage.text = "失 败";
    }

    public void PlayerCancelOptionCallBack()
    {
        SetGamePause(false);
    }

    public void PlayerGameEnd(bool isContinue)
    {
        SetGamePause(!isContinue);
    }

    public void IncreaseScore(int add)
    {
        Score += add;
        scoreText.text = "Score:" + Score.ToString("0000000");
    }
}
