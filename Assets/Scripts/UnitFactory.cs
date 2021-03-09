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

    public UnitAttr[] Attrs { get; private set; } =
    {
        new UnitAttr{ Life = 2.0f,Damage = 1.0f,Skill = 3.0f },
        new UnitAttr{ Life = 3.0f,Damage = 2.0f,Skill = 1.0f },
        new UnitAttr{ Life = 1.0f,Damage = 3.0f,Skill = 2.0f },
    };

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

    public void UpgradeUnitFactory(Unit.Type type)
    {
        // UNDONE: Detailize UpgradeUnitFactory.
        UnitAttr attr = Attrs[(int)type];
        attr.Life++;
        attr.Damage++;
        attr.Skill++;
        Attrs[(int)type] = attr;
    }

    public Unit GenerateUnit(Unit.Type type)
    {
        // TODO: Unit Generation code here.
        return null;
    }
}
