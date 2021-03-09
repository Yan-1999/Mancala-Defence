/// <summary>
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
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //singleton
    static public GameManager Instance { get; private set; } = null;

    public float TimeRate { get; set; } = 1.0f;
    public Map Map { get; private set; } = null;

    private List<Unit> Units { get; set; } = new List<Unit>();
    private List<Enemy> Enemies { get; set; } = new List<Enemy>();
    private PlayerAssets Assets { get; set; } = new PlayerAssets();
    private bool FreeMancala = false;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] gameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject gameObject in gameObjects)
        {
            Map = gameObject.GetComponent<Map>();
            if (Map != null)
            {
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SetGamePause(bool pause)
    {
        if (pause)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = TimeRate;
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
        if (cell.IsVaildForUnitSpawn())
        {
            return false;
        }
        cell.AddUnit(unit);
        unit.OnCell = cell;
        Units.Add(unit);
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
        Units.Remove(unit);
        unit.OnCell.RemoveUnit(unit);
    }

    public void EnemyDeath(Enemy enemy)
    {
        Enemies.Remove(enemy);
        Assets.GainCoinOnEmemyKill();
    }

    public void PlayerCardOption(PlayerOption option)
    {
        List<Unit.Type> types = Assets.CheckHand(option);
        if (types.Count == 0)
        {
            return;
        }
        // let player choose if there are multiple choices
        if (types.Count > 1)
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
        SetGamePause(false);
        Assets.CostCard(type, option);
        switch (option)
        {
            case PlayerOption.UnitUpgrade:
                UnitFactory.Instance.UpgradeUnitFactory(type);
                break;
            case PlayerOption.UnitSpawn:
            case PlayerOption.Mancala:
                SetGamePause(true);
                PlayerInterface.Instance.ChooseCell(option, type);
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
        SetGamePause(false);
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
        Assets.CostCoin(option);
        switch (option)
        {
            case PlayerOption.ExtendHandLimit:
                Assets.HandLimit++;
                break;
            case PlayerOption.UpgradeAttribute:
                SetGamePause(true);
                PlayerInterface.Instance.ChooseUnit();
                break;
            default:
                break;
        }
    }

    public void PlayerChooseUnitCallback(Unit unit, Unit.AttrEnum attrEnum)
    {
        unit.Upgrade(attrEnum);
    }
}
