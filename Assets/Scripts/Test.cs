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
            Debug.Log("you used card");
            GameManager.Instance.PlayerCardTypeCallback(PlayerOption.UnitSpawn, Unit.Type.Red);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("you perform mancala");
            GameManager.Instance.PlayerCardTypeCallback(PlayerOption.Mancala, Unit.Type.Red);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("add coin");
            GameManager.Instance.Assets.coin += 10;
            GameManager.Instance.coinText.text = GameManager.Instance.Assets.coin.ToString();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("enemy in");
            GameManager.Instance.EnemyPass();
        }
    }
}
