using System;
using System.Collections.Generic;

public class PlayerAssets
{
    public const int spwanCost = 2;
    public const int upgradeCost = 3;
    public const int mancalaCost = 3;
    public const int extendHandLimitCost = 100;
    public const int upgradeAttrCost = 30;
    public const int enemyKillGain = 10;

    public LinkedList<Unit.Type> Cards { get; private set; } =
        new LinkedList<Unit.Type>();
    private int[] CardNum { get; set; } = { 0, 0, 0 };
    public int coin { get; set; } = 0;
    public int HandLimit { get; set; } = 5;

    /*card func.s*/

    private void AutoDiscard()
    {
        CardNum[(int)Cards.First.Value]--;
        Cards.RemoveFirst();
    }

    /// <summary>
    /// Draw a card to hand.
    /// Modify <c>Crads</c> and
    /// <c>CardNum</c> simultaneously.
    /// Discard card automatically if
    /// the hand limit is reached.
    /// </summary>
    /// <param name="type">type of crad drawn</param>
    /// <returns>if the hand limit is reached</returns>
    public bool DrawCard(Unit.Type type)
    {
        Cards.AddLast(type);
        CardNum[(int)type]++;
        if (CardNum.Length > HandLimit)
        {
            AutoDiscard();
            return true;
        }
        return false;
    }

    /// <summary>
    /// Check if player can spwan ANY unit.
    /// </summary>
    /// <returns>possible types</returns>
    public List<Unit.Type> CheckSpawn()
    {
        Array arrayEnum = Enum.GetValues(typeof(Unit.Type));
        List<Unit.Type> types = new List<Unit.Type>(arrayEnum.Length);
        foreach (Unit.Type type in arrayEnum)
        {
            if (CardNum[(int)type] > spwanCost)
            {
                types.Add(type);
            }
        }
        return types;
    }

    /// <summary>
    /// Check if player can upgarde ANY unit.
    /// </summary>
    /// <returns>if player can upgarde a unit (no matter what type)</returns>
    public List<Unit.Type> CheckUpgrade()
    {
        Array arrayEnum = Enum.GetValues(typeof(Unit.Type));
        List<Unit.Type> types = new List<Unit.Type>(arrayEnum.Length);
        foreach (Unit.Type type in arrayEnum)
        {
            if (CardNum[(int)type] > upgradeCost)
            {
                types.Add(type);
            }
        }
        return types;
    }

    /// <summary>
    /// Check if player have enough card to
    /// perform Mancala.
    /// </summary>
    /// <returns>if player have enough card to perform Mancala</returns>
    public List<Unit.Type> CheckMancala()
    {
        Array arrayEnum = Enum.GetValues(typeof(Unit.Type));
        List<Unit.Type> types = new List<Unit.Type>(arrayEnum.Length);
        foreach (Unit.Type type in arrayEnum)
        {
            if (CardNum[(int)type] > mancalaCost)
            {
                types.Add(type);
            }
        }
        return types;
    }

    public bool CostCard(Unit.Type type, int num)
    {
        if (CardNum[(int)type] < num)
        {
            return false;
        }

        int counter = 0;
        CardNum[(int)type] -= num;
        LinkedListNode<Unit.Type> card = Cards.First;
        LinkedListNode<Unit.Type> rmCard = null;
        while (counter != num && card != null)
        {
            if (card.Value == type)
            {
                rmCard = card;
                card = card.Next;
                Cards.Remove(card);
                counter++;
            }
            else
            {
                card = card.Next;
            }
        }
        return true;
    }

    /*coin func.s*/

    public void GainCoinOnEmemyKill()
    {
        coin += enemyKillGain;
    }

    public bool CheckExtendHandLimit()
    {
        return coin < extendHandLimitCost;
    }

    public bool CheckAttrUpgrade()
    {
        return coin < upgradeAttrCost;
    }

    public bool ExtendHandLimit()
    {
        if (CheckExtendHandLimit())
        {
            return false;
        }
        coin -= extendHandLimitCost;
        HandLimit++;
        return true;
    }

    public bool CostAttrUpgrade()
    {
        if (CheckAttrUpgrade())
        {
            return false;
        }
        coin -= upgradeAttrCost;
        return true;
    }

}
