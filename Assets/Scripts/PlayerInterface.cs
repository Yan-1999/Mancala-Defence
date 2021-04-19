using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    private PlayerOption highLightOption = PlayerOption.UnitSpawn;//try to differ form another
    private List<Cell> highLightedCells = new List<Cell>();
    public bool IsHighLighting { get; private set; } = false;
    public bool IsUpgrading { get; private set; } = false;
    private Cell cellChosen = null;
    public Unit UnitChosen { get; private set; } = null;
    private Unit.Type nowType = Unit.Type.White;
    public Button upgradeDamage;
    public Button upgradeSkill;
    public Button upgradeLife;
    public Text HintMessage;
    public Button[] chooseType;

    public GameObject endUI;
    public Text endMessage;
    public bool IsChoosingType { get; private set; } = false;
    private PlayerOption chooseTypeOption = PlayerOption.UnitSpawn;
    public Camera upgradeCamera;


    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        upgradeCamera.gameObject.SetActive(false);
        chooseType[0].gameObject.SetActive(false);
        chooseType[1].gameObject.SetActive(false);
        chooseType[2].gameObject.SetActive(false);
        HintMessage.gameObject.SetActive(false);
        upgradeDamage.onClick.AddListener(delegate()
        {
            TryUpgrade(Unit.AttrEnum.Damage);
        });
        upgradeSkill.onClick.AddListener(delegate ()
        {
            TryUpgrade(Unit.AttrEnum.Skill);
        });
        upgradeLife.onClick.AddListener(delegate ()
        {
            TryUpgrade(Unit.AttrEnum.Life);
        });
        chooseType[0].onClick.AddListener(delegate ()
        {
            EndChooseType();
            GameManager.Instance.PlayerCardTypeCallback(chooseTypeOption, Unit.Type.White);
        });
        chooseType[1].onClick.AddListener(delegate ()
        {
            EndChooseType();
            GameManager.Instance.PlayerCardTypeCallback(chooseTypeOption, Unit.Type.Green);
        });
        chooseType[2].onClick.AddListener(delegate ()
        {
            EndChooseType();
            GameManager.Instance.PlayerCardTypeCallback(chooseTypeOption, Unit.Type.Red);
        });
        UpdateUpgradeUI(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(IsChoosingType)
        {
            if (IsUpgrading)
            {
                CancelUpgrade();
            }
            if (Input.GetMouseButtonDown(1))
            {
                GameManager.Instance.PlayerCancelOptionCallBack();
                EndChooseType();
            }
        }
        else if(IsHighLighting)
        {
            if(IsUpgrading)
            {
                CancelUpgrade();
            }
            if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("Map")))
                {
                    cellChosen = hit.collider.GetComponent<Cell>();
                    if (highLightedCells.Contains(cellChosen))
                    {
                        GameManager.Instance.PlayerChooseCellCallback(highLightOption, nowType, cellChosen);
                        HighLightCellsEnd();
                    }
                    else
                    {
                        SetHintMessage("Invalid cell is chosen");
                        HighLightCellsEnd();
                        GameManager.Instance.PlayerCancelOptionCallBack();
                    }
                }
            }
            else if(Input.GetMouseButtonDown(1))
            {
                HighLightCellsEnd();
                GameManager.Instance.PlayerCancelOptionCallBack();
            }
        }
        else if(IsUpgrading)
        {
            if (Input.GetMouseButtonDown(1))
            {
                CancelUpgrade();
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
        // Call PlayerCardTypeCallback(PlayerOption, Unit.Type)
        // when player click the confirm button.
        foreach(Unit.Type type in types)
        {
            chooseType[(int)type].gameObject.SetActive(true);
        }
        IsChoosingType = true;
        chooseTypeOption = option;
    }

    public void EndChooseType()
    {
        foreach(Button color in chooseType)
        {
            color.gameObject.SetActive(false);
        }
        IsChoosingType = false;
        Debug.Log("choose type end");
    }

    private void TryUpgrade(Unit.AttrEnum attrEnum)
    {
        if (UnitChosen != null)
        {
            if (GameManager.Instance.Assets.CostCoin(PlayerOption.UpgradeAttribute))
            {
                IsUpgrading = false;
                GameManager.Instance.PlayerChooseUnitCallback(UnitChosen, attrEnum);
                UnitChosen = null;
                UpdateUpgradeUI(true);
                Debug.Log("upgrade");
                Debug.Log(attrEnum);
            }
            else
            {
                CancelUpgrade();
                ShowNoEnoughResource(false);
            }
        }
    }

    public void CancelUpgrade()
    {
        IsUpgrading = false;
        UnitChosen = null;
        Debug.Log("you cancelled upgrade");
        UpdateUpgradeUI(true);
    }
    /// <summary>
    /// Show all possible cell for player to choose.
    /// </summary>
    /// <param name="option">player option</param>
    /// <param name="types">chosen types</param>
    public void HighLightCells(PlayerOption option, Unit.Type type)
    {
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
        highLightOption = option;
        IsHighLighting = true;
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
        if (highLightedCells.Count == 0)
        {
            GameManager.Instance.PlayerCancelOptionCallBack();
            HighLightCellsEnd();
        }
    }

    private void HighLightCellsEnd()
    {
        foreach (Cell cell in highLightedCells)
        {
            cell.HighLightEnd();
        }
        highLightedCells.Clear();
        nowType = Unit.Type.White;
        IsHighLighting = false;
        cellChosen = null;
    }

    /// <summary>
    /// Wait for player to choose unit.
    /// </summary>
    public void ChooseUnit()
    {
        // Call PlayerChooseUnitCallback(Unit, Unit.AttrEnum)
        // when player click a unit.
        if(EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("Units")))
        {
            UnitChosen = hit.collider.GetComponent<Unit>();
            if (UnitChosen!=null)
            {
                //UNDONE:highlight?
                IsUpgrading = true;
                UpdateUpgradeUI(false);
            }
        }
    }

    private void UpdateUpgradeUI(bool iscancel)
    {
        GameManager.Instance.mainCamera.gameObject.SetActive(iscancel);
        upgradeCamera.gameObject.SetActive(!iscancel);
        upgradeSkill.gameObject.SetActive(!iscancel);
        upgradeLife.gameObject.SetActive(!iscancel);
        upgradeDamage.gameObject.SetActive(!iscancel);
        if (!iscancel)
        {
            upgradeCamera.transform.position = UnitChosen.transform.position + new Vector3(0, 3, -0.5f);
            upgradeSkill.GetComponentInChildren<Text>().text = "Skill:" + UnitChosen.Skill.ToString() + '+';
            upgradeDamage.GetComponentInChildren<Text>().text = "Damage:" + UnitChosen.Damage.ToString() + '+';
            upgradeLife.GetComponentInChildren<Text>().text = "Life:" + UnitChosen.Life.ToString() + '/' + UnitChosen.LifeLimit.ToString() + '+';
        }
    }

    /*Notice windows*/
    /// <summary>
    /// Pop a warning window to show there are not enough
    /// </summary>
    /// <param name="isCard"></param>
    public void ShowNoEnoughResource(bool isCard)
    {
        if(isCard)
        {
            SetHintMessage("No enough cards!");
        }
        else
        {
            SetHintMessage("No enough coins!");
        }
    }

    private void SetHintMessage(string text)
    {
        CancelInvoke("TextActivefalse");
        HintMessage.text = text;
        HintMessage.gameObject.SetActive(true);
        Invoke("TextActivefalse", 2f);
    }

    private void TextActivefalse()
    {
        HintMessage.gameObject.SetActive(false);
    }

    //Game Over
    public void Failed()
    {
        endUI.SetActive(true);
        endMessage.text = "失 败";
    }

    public void Win()
    {
        endUI.SetActive(true);
        endMessage.text = "胜 利";
    }

    public void OnButtonRetry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void OnButtonReturn()
    {
        SceneManager.LoadScene(0);
    }
}
