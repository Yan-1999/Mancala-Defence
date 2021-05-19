using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guide : MonoBehaviour
{
    public int index = 0;
    public GameObject[] step;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void DisCardAll()
    {
        GameManager.Instance.Assets.GuideDiscard();
    }

    private void DrawCard(Unit.Type type)
    {
        GameManager.Instance.Assets.DrawCard(type);
    }

    private void ChangeCoin(int amount)
    {
        GameManager.Instance.Assets.coin += amount;
    }

    private void Introduce()
    {
        DisCardAll();
        DrawCard(Unit.Type.White);
        DrawCard(Unit.Type.Green);
        DrawCard(Unit.Type.Red);
    }

    private void Spawn()
    {
        DisCardAll();
        DrawCard(Unit.Type.Red);
        DrawCard(Unit.Type.Red);
        DrawCard(Unit.Type.Red);
        DrawCard(Unit.Type.Red);
        DrawCard(Unit.Type.Green);
        DrawCard(Unit.Type.Green);
    }


    private void Mancala()
    {
        DisCardAll();
        DrawCard(Unit.Type.White);
        DrawCard(Unit.Type.White);
        DrawCard(Unit.Type.White);
    }
    private void BaseCell()
    {
        DisCardAll();
        DrawCard(Unit.Type.White);
        DrawCard(Unit.Type.White);
        DrawCard(Unit.Type.White);

    }

    private void HandLimit()
    {
        DisCardAll();
        GameManager.Instance.DrawCard();
        GameManager.Instance.DrawCard();
        GameManager.Instance.DrawCard();
        GameManager.Instance.DrawCard();
        GameManager.Instance.DrawCard();
        ChangeCoin(-GameManager.Instance.Assets.coin);
        ChangeCoin(100);

    }

    private void FreeMancala()
    {
        DisCardAll();
    }

    private void Vulnerable()
    {
        DisCardAll();
        DrawCard(Unit.Type.Red);
        DrawCard(Unit.Type.Red);
    }



    private void Upgrade()
    {
        DisCardAll();
        DrawCard(Unit.Type.White);
        DrawCard(Unit.Type.White);
        DrawCard(Unit.Type.White);

    }

    private void UpgradeUnit()
    {
        DisCardAll();
        ChangeCoin(-GameManager.Instance.Assets.coin);
        ChangeCoin(30);

    }
}
