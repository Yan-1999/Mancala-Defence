using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct UnitAttr
{
    public float Life { get; set; }
    public float Damage { get; set; }
    public float Skill { get; set; }
}


public class UnitFactory : MonoBehaviour
{
    static public UnitFactory Instance = null;
    public Unit[] origin;

    public UnitAttr[] Attrs { get; private set; } =
    {
        new UnitAttr{ Life = 200f,Damage = 1f,Skill = 3.0f },
        new UnitAttr{ Life = 300f,Damage = 2f,Skill = 1.0f },
        new UnitAttr{ Life = 100f,Damage = 3f,Skill = 2.0f },
    };

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            origin[i].SetUnitAttrs(Attrs[i]);
            origin[i].UnitType = (Unit.Type)i;
            origin[i].gameObject.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpgradeUnitFactory(Unit.Type type)
    {
        // UNDONE: Detailize UpgradeUnitFactory.
        UnitAttr attr = Attrs[(int)type];
        attr.Life++;
        attr.Damage++;
        attr.Skill++;
        Attrs[(int)type] = attr;
        CardFactory.Instance.UpdateCardAttribute(type);
    }

    public Unit GenerateUnit(Unit.Type type)
    {
        return origin[(int)type];
    }
}
