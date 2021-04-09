using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CardFactory : MonoBehaviour
{
    static public CardFactory Instance = null;
    public Card[] origin;
    public Image cardArea;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            origin[i].gameObject.SetActive(false);
        }
    }

    public Card DrawCard(Unit.Type type)
    {
        Card newCard = Instantiate<Card>(origin[(int)type]);
        newCard.gameObject.SetActive(true);
        newCard.transform.SetParent(cardArea.transform);
        newCard.transform.rotation = cardArea.transform.rotation;
        return newCard;
    }

}
