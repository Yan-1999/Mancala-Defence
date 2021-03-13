using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//每一波生成敌人的信息
[System.Serializable]
public class Wave
{
    public GameObject enemyPrefab;
    public int count;
    public float rate;
}
