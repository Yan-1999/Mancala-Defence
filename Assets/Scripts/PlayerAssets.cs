using System;
using System.Collections.Generic;

public class PlayerAssets
{
    public readonly int[] Costs = new int[]
    {
        2, // UnitSpawn
        3, // UnitUpgrade
        3, // Mancala
        100, // ExtendHandLimit
        30 // UpgradeAttribute
    };
    public const int enemyKillGain = 20;

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
    /// Check if player have enough card.
    /// </summary>
    /// <returns>return the type which player have enough card</returns>
    public List<Unit.Type> CheckHand(PlayerOption option)
    {
        Array arrayEnum = Enum.GetValues(typeof(Unit.Type));
        List<Unit.Type> types = new List<Unit.Type>(arrayEnum.Length);
        foreach (Unit.Type type in arrayEnum)
        {
            if (CardNum[(int)type] > Costs[(int)option])
            {
                types.Add(type);
            }
        }
        return types;
    }

    public bool CostCard(Unit.Type type, PlayerOption option)
    {
        int num = Costs[(int)option];
        if (CardNum[(int)type] < num)
        {
            return false;
        }

        int counter = 0;
        CardNum[(int)type] -= num;
        LinkedListNode<Unit.Type> card = Cards.First;
        LinkedListNode<Unit.Type> rmCard;
        while (counter != num && card != null)
        {
            if (card.Value == type)
            {
                rmCard = card;
                card = card.Next;
                Cards.Remove(rmCard);
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

    public bool CheckCoin(PlayerOption option)
    {
        return coin < Costs[(int)option];
    }

    /// <summary>
    /// For UpgredAttr, do ONLY the coin consume step.
    /// </summary>
    /// <param name="option">option</param>
    /// <returns>if the option succeeded</returns>
    public bool CostCoin(PlayerOption option)
    {
        if (CheckCoin(option))
        {
            return false;
        }
        coin -= Costs[(int)option];
        return true;
    }
}
