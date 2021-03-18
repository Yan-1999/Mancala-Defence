﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum PlayerOption
{
    UnitSpawn = 0,
    UnitUpgrade,
    Mancala,
    ExtendHandLimit,
    UpgradeAttribute
}

public class PlayerInterface : MonoBehaviour
{

    public static PlayerInterface Instance { get; private set; } = null;
    private PlayerOption nowOption = PlayerOption.UnitSpawn;
    private List<Cell> highLightedCells = new List<Cell>();
    private bool isHighLighting = false;
    private Cell cellChosen = null;
    private Unit.Type nowType = Unit.Type.White;

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
        if(isHighLighting)
        {
            if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("Map")))
                {
                    cellChosen = hit.collider.GetComponent<Cell>();//right or wrong?
                    if (highLightedCells.Contains(cellChosen))
                    {
                        GameManager.Instance.PlayerChooseCellCallback(nowOption, nowType, cellChosen);
                        Debug.Log("you successfully chose the cell");
                        HighLightCellsEnd();
                    }
                    else
                    {
                        //TODO:warning wrong action and continue
                    }
                }
            }
            else if(Input.GetMouseButtonDown(1))
            {
                HighLightCellsEnd();
            }
        }
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
    public void HighLightCells(PlayerOption option, Unit.Type type)
    {
        // TODO: UI for choose cells.
        // Implementation steps:
        //     1. Highlight all vaild (and invaild) cells.
        //        NOTICE that this func. can be used
        //        for unit spawn and Mancala. These have
        //        different vaildation checks.(finished)
        //     2. Wait for player click (slow down or stop if needed).
        //        After player click a cell,
        //        check which cell is clicked by player.
        //        If player canceled at this step, return.
        //        (and clear highlights).
        //     3. If the cell is vaild(highlighted),
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
        //     - GameManager.TimeRate: float(not int,corrected)
        //       Game time elapse rate. You can modify it.
        nowOption = option;
        isHighLighting = true;
        nowType = type;
        if(option==PlayerOption.UnitSpawn)
        {
            foreach(Cell cell in GameManager.Instance.Map.Cells)
            {
                if (cell.IsVaildForUnitSpawn())//able to spawn
                {
                    highLightedCells.Add(cell);
                }
            }
        }
        else if(option==PlayerOption.Mancala)
        {
            foreach (Cell cell in GameManager.Instance.Map.Cells)
            {
                if (cell.UnitCount() > 0)//have units on target cell
                {
                    highLightedCells.Add(cell);
                }
            }
        }
        else
        {
            return;
        }
        foreach(Cell cell in highLightedCells)
        {
            cell.HighLight();
        }
        Debug.Log("before choose cells,cells are highlighted");
    }

    private void HighLightCellsEnd()
    {
        foreach (Cell cell in highLightedCells)
        {
            cell.HighLightEnd();
        }
        highLightedCells.Clear();
        nowType = Unit.Type.White;
        isHighLighting = false;
        cellChosen = null;
        Debug.Log("after choosing cell or cancelled,highlight end");
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

    //Game Over
    public void Failed()
    {

    }
}
