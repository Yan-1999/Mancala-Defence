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
    }
}
