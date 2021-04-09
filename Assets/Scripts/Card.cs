using UnityEngine;


public class Card : MonoBehaviour
{
    public Unit.Type type;


    public void UseCard()
    {
        Debug.Log("card used or discard");
        Destroy(gameObject);
    }
}