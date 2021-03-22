/// <summary>
/// The "base" cell.
/// Author: MjuTeX
/// Project: Mancala Defence
/// File: BaseCell.cs
/// </summary>
/// 
using UnityEngine;

public class BaseCell : Cell
{
    static public float TimerMax = 10;

    private float timer = 0.0f;
    private void Update()
    {
        float deltaTime = Time.deltaTime;
        OnUpdate(deltaTime);
    }

    /// <summary>
    /// This is an override func. on <c>Cell.CheckOverload()</c>
    /// This func. is blank func. 
    /// because <c>Base</c> will never be "overloaded".
    /// </summary>
    /// <param name="deltaTime"></param>
    /// <seealso cref="Cell.CheckOverload(float)"/>
    protected override void CheckOverload(float deltaTime) { }

    private void OnUpdate(float deltaTime)
    {
        timer += deltaTime * Units.Count;
        if (timer > TimerMax)
        {
            timer = 0.0f;
            GameManager.Instance.DrawCard();
        }
    }
}
