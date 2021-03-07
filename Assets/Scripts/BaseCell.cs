/// <summary>
/// The "base" cell.
/// Author: MjuTeX
/// Project: Mancala Defence
/// File: BaseCell.cs
/// </summary>

public class BaseCell : Cell
{
    /// <summary>
    /// This is an override func. on <c>Cell.CheckOverload()</c>
    /// This func. is blank func. 
    /// because <c>Base</c> will never be "overloaded".
    /// </summary>
    /// <param name="deltaTime"></param>
    /// <seealso cref="Cell.CheckOverload(float)"/>
    protected override void CheckOverload(float deltaTime) { }
}
