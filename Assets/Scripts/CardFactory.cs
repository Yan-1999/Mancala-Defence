using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CardFactory : MonoBehaviour
{
    static public CardFactory Instance = null;
    public Card[] origin;
    public Image cardArea;
    float lastWidth = 0;
    float lastHeight = 0;

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

    private void OnGUI()
    {
        if (lastHeight != Screen.height || lastWidth != Screen.width)
        {
            lastWidth = Screen.width;
            lastHeight = Screen.height;
            //cardArea.rectTransform.sizeDelta = new Vector2(Screen.width * 3 / 32, Screen.height * 5 / 6);
            //cardArea.GetComponent<GridLayoutGroup>().cellSize = new Vector2(Screen.width * 3 / 32, Screen.height / 6);
            //cardArea.GetComponent<GridLayoutGroup>().spacing = new Vector2(0, -Screen.height / 12);
        }
    }

    public Card DrawCard(Unit.Type type)
    {
        Card newCard = Instantiate<Card>(origin[(int)type]);
        newCard.type = type;
        newCard.gameObject.SetActive(true);
        newCard.transform.SetParent(cardArea.transform);
        newCard.transform.localScale = new Vector3(1, 1, 1);
        newCard.GetComponentInChildren<Text>().text= UnitFactory.Instance.Attrs[(int)type].Damage.ToString() + '-' +
                    UnitFactory.Instance.Attrs[(int)type].Life.ToString() + '-' + UnitFactory.Instance.Attrs[(int)type].Skill.ToString();
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

    public void UpdateCardAttribute(Unit.Type type)
    {
        LinkedListNode<Card> presentNode = GameManager.Instance.Assets.CardsImage.First;
        while(presentNode!=null)
        {
            if(presentNode.Value.type==type)
            {
                presentNode.Value.GetComponentInChildren<Text>().text = UnitFactory.Instance.Attrs[(int)type].Damage.ToString() + '-' +
                    UnitFactory.Instance.Attrs[(int)type].Life.ToString() + '-' + UnitFactory.Instance.Attrs[(int)type].Skill.ToString();
            }
            presentNode = presentNode.Next;
        }
    }
}
