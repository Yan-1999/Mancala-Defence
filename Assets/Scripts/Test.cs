using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log("test start");
            for (int i = 0; i < 3; i++)
            {
                GameManager.Instance.Assets.DrawCard(Unit.Type.Green);
            }
            GameManager.Instance.PlayerCardOption(PlayerOption.UnitSpawn);
        }
        //codes below are just for test
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //PlayerChooseCellCallback(PlayerOption.UnitSpawn, Unit.Type.Green, Map.Cells[0]);
            GameManager.Instance.PlayerCardTypeCallback(PlayerOption.UnitSpawn, Unit.Type.Red);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            GameManager.Instance.PlayerCardTypeCallback(PlayerOption.Mancala, Unit.Type.Red);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("add coin");
            GameManager.Instance.Assets.coin += 100;
            GameManager.Instance.coinText.text = GameManager.Instance.GetFrontZero(GameManager.Instance.Assets.coin, true);
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("enemy in");
            GameManager.Instance.EnemyPass();
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            GameManager.Instance.DrawCard();
        }
    }
}
