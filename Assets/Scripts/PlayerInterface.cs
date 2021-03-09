using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerOption
{
    UnitSpawn,
    UnitUpgrade,
    Mancala,
    ExtendHandLimit,
    UpgradeAttribute
}

public class PlayerInterface : MonoBehaviour
{

    public static PlayerInterface Instance { get; private set; } = null;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Pop a window to ask which type of card
    /// to consume.
    /// Player can cancel operation here.
    /// </summary>
    /// <param name="option">player option</param>
    /// <param name="types">possible card types</param>
    public void ChooseType(PlayerOption option, List<Unit.Type> types)
    {
        // TODO: UI for player to choose type.
        // Call PlayerCardTypeCallback(PlayerOption, Unit.Type)
        // when player click the confirm button.
    }


    /// <summary>
    /// Show all possible cell for player to choose.
    /// </summary>
    /// <param name="option">player option</param>
    /// <param name="types">chosen types</param>
    public void ChooseCell(PlayerOption option, Unit.Type type)
    {
        // TODO: UI for choose cells.
        // Implementation steps:
        //     1. Highlight all vaild (and invaild) cells.
        //        NOTICE that this func. can be used
        //        for unit spawn and Mancala. These have
        //        different vaildation checks.
        //     2. Wait for player click (slow down or stop if needed).
        //        After player click a cell,
        //        check which cell is clicked by player.
        //        If player canceled at this step, return.
        //        (and clear highlights).
        //     3. If the cell is vaild,
        //        call PlayerChooseCellCallback(PlayerOption, Unit.Type, Cell).
        //        Else pop warning message, and back to step 2.
        // Helper functions:
        //     - GameManager.Instance.Map.Cells: Cell[]
        //       get all the cells
        //     - Cell.IsVaildForUnitSpawn(): bool
        //       check cell is vaild for unit spawn
        //     - Cell.UnitCount(): int
        //       Return the unit count on the cell. Can be used
        //       in Mancala vaild check.
        //     - GameManager.TimeRate: int
        //       Game time elapse rate. You can modify it.
    }

    /// <summary>
    /// Wait for player to choose unit.
    /// </summary>
    public void ChooseUnit()
    {
        // TODO: UI for choose unit.
        // Call PlayerChooseUnitCallback(Unit, Unit.AttrEnum)
        // when player click a unit.
    }

    /*Notice windows*/
    /// <summary>
    /// Pop a warning window to show there are not enough
    /// </summary>
    /// <param name="isCard"></param>
    public void ShowNoEnoughResource(bool isCard)
    {
        // TODO: pop a window to show
    }
}
