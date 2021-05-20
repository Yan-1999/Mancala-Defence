using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public struct EnemyAttr
{
    public float Life { get; set; }
    public float Damage { get; set; }
    public float Speed { get; set; }
}*/

public class EnemySpawner : MonoBehaviour
{

    /*static public EnemySpawner Instance = null;
    public Enemy[] origin;
    public EnemyAttr[] Attrs { get; private set; } =
    {
        new EnemyAttr{ Life = 150.0f,Damage = 1.0f,Speed = 1.0f },
        new EnemyAttr{ Life = 250.0f,Damage = 2.0f,Speed = 2.0f },
    };
    private void Awake()
    {
        Instance = this;
    }*/

    public static int CountEnemyAlive = 0;
    public GameObject playerinterface;
    public Wave[] waves;
    public Transform START;
    public float waveRate = 3;
    public float BigwaveRate = 3;
    private int presentWave=0;

    void Start()
    {
        StartCoroutine("SpawnEnemy");
    }

    public void Stop()
    {
        StopCoroutine("SpawnEnemy");
    }

    IEnumerator SpawnEnemy()
    {
        foreach(Wave wave in waves)
        {
            
            if (presentWave % 2 != 0)
            {
                yield return new WaitForSeconds(waveRate);
            }
            else
            {
                yield return new WaitForSeconds(BigwaveRate);
            }
            GameManager.Instance.presentWaveText.text = (presentWave + 1).ToString("00") + "/";
            for (int i=0;i<wave.count;i++)
            {
                if (i != 0)
                 {
                     yield return new WaitForSeconds(wave.rate);
                 }
                GameObject.Instantiate(wave.enemyPrefab, START.position, Quaternion.identity);
                CountEnemyAlive++;
                
            }
            /*while(CountEnemyAlive>0)
            {
                yield return 0;
            }*/
            
            
            presentWave++;
        }
        while(CountEnemyAlive>0)
        {
            yield return 0;
        }
        
        PlayerInterface.Instance.Win();
        Stop();
    }

}
