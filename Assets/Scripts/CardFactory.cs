﻿using UnityEngine;
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
        newCard.transform.position = cardArea.transform.position + new Vector3(0, 330 - 90 * GameManager.Instance.Assets.CardsImage.Count);
        
        return newCard;
    }

    public void ResetCardPosition()
    {
        int count = GameManager.Instance.Assets.CardsImage.Count;
        LinkedListNode<Card> presentNode = GameManager.Instance.Assets.CardsImage.First;
        for (int i = 0; i < count && presentNode != null; i++)
        {
            presentNode.Value.transform.position = cardArea.transform.position + new Vector3(0, 330 - 90 * i);
            presentNode = presentNode.Next;
        }
    }
}
