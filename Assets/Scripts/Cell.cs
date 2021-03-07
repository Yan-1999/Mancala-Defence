﻿/// <summary>
/// Author: MjuTeX
/// Project: Mancala Defence
/// File: Cell.cs
/// </summary>

using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{

    private const float overloadTimerMax = 10.0f;
    private const float overloadUnitNum = 3.0f;
    private const float overloadRate = 1.0f;
    public const float vulAttackMultiplyer = 0.8f;

    private float OverloadTimer { get; set; } = 0;
    public bool IsExhausted { get; private set; } = false;
    private List<Unit> Units { get; set; } = new List<Unit>();
    public bool IsVulnerable { get; set; } = false;
    public bool Acivated { get; private set; } = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = GameManager.Instance.deltaTime();
        if (deltaTime > 0)
        {
            CheckOverload(deltaTime);
        }
    }

    /*operations for units on cell*/

    /// <summary>
    /// Construct a <c>Unit[]</c> array of <c>Units</c> and return.
    /// </summary>
    /// <returns>constucted <c>Unit[]</c> array</returns>
    public Unit[] UnitArrayCopy()
    {
        Unit[] units = new Unit[Units.Count];
        Units.CopyTo(units);
        return units;
    }

    /// <summary>
    /// Add a unit to this cell.
    /// This operation will activate cell.
    /// </summary>
    /// <see cref="GameMechanism.UnitSpawn(Unit, Cell)"/>
    /// <param name="unit">unit to add</param>
    public void AddUnit(Unit unit)
    {
        Units.Add(unit);
        Acivated = true;
    }

    /// <summary>
    /// Remove ONE UNIT form this cell. <br />
    /// </summary>
    /// <see cref="GameMechanism.UnitDeath(Unit, Cell)"/>
    /// <param name="unit">unit to remove</param>
    public void RemoveUnit(Unit unit)
    {
        Units.Remove(unit);
    }

    /// <summary>
    /// Clear all units.
    /// </summary>
    /// <see cref="GameMechanism.Mancala(Cell)"/>
    public void ClearUnits()
    {
        Units.Clear();
    }

    /*"overload" func.s*/

    /// <summary>
    /// Called by <c>Update()</c> to check overloading.
    /// This func.is set to be <c>protected virtual</c>
    /// because <c>Base</c> will override this func.
    /// making itself never overloaded.
    /// </summary>
    /// <see cref="Update"/>
    /// <param name="deltaTime"></param>
    /// <seealso cref="BaseCell.CheckOverload(float)"/>
    protected virtual void CheckOverload(float deltaTime)
    {
        if (IsExhausted)
        {
            if (Units.Count == 0)
            {
                IsExhausted = false;
                OverloadTimer = 0;
            }
        }
        else
        {
            if (Units.Count > overloadUnitNum)
            {
                Overloading(deltaTime);
            }
            else
            {
                OverloadTimer = 0;
            }
        }
    }

    /// <summary>
    /// Called by <c>CheckOverload()</c> when "overloading",
    /// that is, <c>Units.Count > overloadUnitNum</c>.<br />
    /// Add <c>deltaTime * Units.Count * overloadRate</c>
    /// to <c>OverloadTimer</c>.
    /// </summary>
    /// <param name="deltaTime">time elapsed</param>
    /// <see cref="CheckOverload"/>
    private void Overloading(float deltaTime)
    {
        OverloadTimer += deltaTime * Units.Count * overloadRate;
        if (OverloadTimer > overloadTimerMax)
        {
            OnExhaustion();
            OverloadTimer = 0;
        }
    }

    /// <summary>
    /// Called by <c>CheckOverload()</c> when "exhausted".
    /// </summary>
    /// <see cref="CheckOverload"/>
    private void OnExhaustion()
    {
        IsExhausted = true;
    }
}
