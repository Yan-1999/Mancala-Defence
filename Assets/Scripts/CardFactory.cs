using UnityEngine;
using UnityEngine.UI;

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
        newCard.transform.position = cardArea.transform.position + new Vector3(-1, 0, 3 - 0.5f * GameManager.Instance.Assets.CardsImage.Count);
        Debug.Log(GameManager.Instance.Assets.CardsImage.Count);
        return newCard;
    }
}
