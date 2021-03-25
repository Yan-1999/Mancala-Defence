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
    public GameObject playerCanvas;
    public Button pauseButton;
    private bool isPause = false;
    public Text coinText;
    public Text lifeText;
    public Button spawnButton;
    public Button mancalaButton;
    public Button unitUpgradeButton;

    private List<Unit> Units { get; set; } = new List<Unit>();
    private List<Enemy> Enemies { get; set; } = new List<Enemy>();
    public PlayerAssets Assets = new PlayerAssets();
    private bool FreeMancala = false;

    public Vector3[] presetPosition;

    public float PlayerHp = 10;
    public GameObject endUI;
    public Text endMessage;

    private System.Random random = new System.Random();


    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] gameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        pauseButton.onClick.AddListener(delegate ()
        {
            SetGamePause(!isPause);
        });
        spawnButton.onClick.AddListener(delegate ()
        {
            if (PlayerInterface.Instance.IsHighLighting || PlayerInterface.Instance.IsChoosingType)
            {
                return;
            }
            PlayerCardOption(PlayerOption.UnitSpawn);
        });
        mancalaButton.onClick.AddListener(delegate ()
        {
            if (PlayerInterface.Instance.IsHighLighting || PlayerInterface.Instance.IsChoosingType)
            {
                return;
            }
            PlayerCardOption(PlayerOption.Mancala);
        });
        unitUpgradeButton.onClick.AddListener(delegate ()
        {
            if (PlayerInterface.Instance.IsHighLighting || PlayerInterface.Instance.IsChoosingType)
            {
                return;
            }
            PlayerCardOption(PlayerOption.UnitUpgrade);
        });
    }

    // Update is called once per frame
    void Update()
    {
        //codes below should be preserved
        coinText.text = Assets.coin.ToString();
        lifeText.text = PlayerHp.ToString();
        if(!PlayerInterface.Instance.IsHighLighting)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PlayerInterface.Instance.ChooseUnit();
            }
        }
        //UNDONE:add check for buttons
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
        Debug.Log("unit moved");
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
        Unit newUnit = Instantiate(unit, cell.transform.position + presetPosition[cell.UnitCount() % 9], Quaternion.identity);
        newUnit.UnitType = unit.UnitType;
        newUnit.Life = UnitFactory.Instance.Attrs[(int)newUnit.UnitType].Life;
        newUnit.Damage = UnitFactory.Instance.Attrs[(int)newUnit.UnitType].Damage;
        newUnit.Skill = UnitFactory.Instance.Attrs[(int)newUnit.UnitType].Skill;
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
        // UNDONE: Special rule oppositing for Mancala.
        int cellIndex = Array.IndexOf(Map.Cells, cell);
        Unit[] units = cell.UnitArrayCopy();
        cell.ClearUnits();
        foreach (Unit unit in units)
        {
            cellIndex = (cellIndex + 1) % Map.Cells.Length;
            if (unit == units[units.Length - 1] &&
                Map.Cells[cellIndex].UnitCount() == 0)
            {
                mancalaButton.image.color = Color.cyan;
                FreeMancala = true;
            }
            UnitMove(unit, Map.Cells[cellIndex]);
        }
    }

    /*attack*/
    public void UnitAttack(Unit unit)
    {
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
        }
    }

    public void EnemyAttack(Enemy enemy)
    {
        List<Unit> units = GameAI.FindUnitsToAttack(enemy);
        foreach (Unit unit in units)
        {
            unit.ReceiveDamage(enemy.Damage);
        }
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
            Debug.Log("no enough card");
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
        Debug.Log("game continue");
        if (FreeMancala && option == PlayerOption.Mancala)
        {
            Debug.Log("you perform free mancala");
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
                break;
            case PlayerOption.Mancala:
                Mancala(cell);
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
        Debug.Log("you draw a card");
        Assets.DrawCard(
            (Unit.Type)random.Next(0, Enum.GetValues(typeof(Unit.Type)).Length)
            );
    }


    public void EnemyPass()
    {
        PlayerHp--;
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
        GameObject.Find("EnemySpawner").SendMessage("Stop");
        //endUI.SetActive(true);
        //endMessage.text = "失 败";
    }

    public void PlayerCancelOptionCallBack()
    {
        SetGamePause(false);
    }
}
