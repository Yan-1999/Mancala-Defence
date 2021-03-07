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

    private Map Map { get; set; } = null;
    private List<Unit> Units { get; set; } = new List<Unit>();
    private List<Enemy> Enemies { get; set; } = new List<Enemy>();
    private PlayerAssets assets { get; set; } = new PlayerAssets();

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

    public float deltaTime()
    {
        return Time.deltaTime * TimeRate;
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
    /// Spwan a unit on cell.
    /// This will check whether the cell is base
    /// or inactivate vul..
    /// </summary>
    /// <param name="unit">unit to spawn</param>
    /// <param name="cell">cell to place on</param>
    /// <returns>whether the spwan is successful</returns>
    public bool UnitSpawn(Unit unit, Cell cell)
    {
        if (cell.GetType().Equals(typeof(BaseCell))
            || (cell.IsVulnerable && cell.Acivated)
            )
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
        int cellIndex = Array.IndexOf(Map.Cells, cell);
        Unit[] units = cell.UnitArrayCopy();
        cell.ClearUnits();
        foreach (Unit unit in units)
        {
            cellIndex = (cellIndex + 1) % Map.Cells.Length;
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
        assets.GainCoinOnEmemyKill();
    }
}
