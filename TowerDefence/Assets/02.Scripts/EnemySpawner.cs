using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float startDelay = 1f;
    [HideInInspector] public int currentStage;

    [System.Serializable]
    public class SpawnElement
    {
        public GameObject prefab;
        public int num;
        public float delay;
    }
    [SerializeField] private SpawnElement[][] spawnElementsList;
    private float[][] timers;
    private int[][] counts;

    public void Spawn()
    {
        if (currentStage < spawnElementsList.Length - 1)
        {
            StartCoroutine(E_Spawn());
            currentStage++;
        }
    }

    private void Awake()
    {
        timers = new float[spawnElementsList.Length][];
        counts = new int[spawnElementsList.Length][];

        for (int i = 0; i < spawnElementsList.Length; i++)
        {
            timers[i] = new float[spawnElementsList[i].Length];
            counts[i] = new int[spawnElementsList[i].Length];

            for (int j = 0; j < spawnElementsList[i].Length; j++)
            {
                timers[i][j] = spawnElementsList[i][j].delay;
                counts[i][j] = spawnElementsList[i][j].num;
            }
        }
    }

    private IEnumerator E_Spawn()
    {
        yield return new WaitForSeconds(startDelay);

        for (int i = 0; i < spawnElementsList[currentStage].Length; i++)
        {
            // ��ȯ�Ұ��� �����ִ��� üũ
            if (counts[currentStage][i] > 0)
            {
                // ��ȯ ������ üũ
                if (timers[currentStage][i] < 0)
                {
                    Instantiate(spawnElementsList[currentStage][i].prefab,
                                WayPoints.instance.GetFirstWayPoint().position,
                                Quaternion.identity);
                    counts[currentStage][i]--;
                    timers[currentStage][i] = spawnElementsList[currentStage][i].delay;
                }
                else
                    timers[currentStage][i] -= Time.deltaTime;
            }
        }
    }


}
