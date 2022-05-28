using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float startDelay = 1f;
    [HideInInspector] public int currentLevel;
    [HideInInspector] public int currentStage;

    [System.Serializable]
    public class SpawnElement
    {
        public GameObject prefab;
        public int num;
        public float delay;
    }
    [SerializeField] private SpawnElement[][] spawnElements;
    private float[][] timers;
    private int[][] counts;

    public void Spawn()
    {
        if (currentStage < spawnElements.Length)
        {
            StartCoroutine(E_Spawn());           
        }
    }

    private void Awake()
    {
        // ���� ������ ���� ��� �������� ���� ������
        StageInfo[] tmpStageInfos = LevelInfoAssets.GetAllStageInfo(currentLevel);

        // ��ȯ�ؾ��ϴ� ���ʹ� �迭�� �������� ũ�� �Ҵ�
        spawnElements = new SpawnElement[tmpStageInfos.Length][];

        // ��ȯ�ؾ��ϴ� ���������� ���ʹ� ��� �Ҵ�
        for (int i = 0; i < tmpStageInfos.Length; i++)
        {
            spawnElements[i] = tmpStageInfos[i].enemiesElements;
        }

        timers = new float[spawnElements.Length][];
        counts = new int[spawnElements.Length][];

        for (int i = 0; i < spawnElements.Length; i++)
        {
            timers[i] = new float[spawnElements[i].Length];
            counts[i] = new int[spawnElements[i].Length];

            for (int j = 0; j < spawnElements[i].Length; j++)
            {
                timers[i][j] = spawnElements[i][j].delay;
                counts[i][j] = spawnElements[i][j].num;
            }
        }
    }

    private IEnumerator E_Spawn()
    {
        int tmpStage = currentStage;
        currentStage++;
        yield return new WaitForSeconds(startDelay);

        bool isDone = false;
        while (isDone == false)
        {
            isDone = true;

            for (int i = 0; i < spawnElements[tmpStage].Length; i++)
            {
                // ��ȯ�Ұ��� �����ִ��� üũ
                if (counts[tmpStage][i] > 0)
                {
                    isDone = false;

                    // ��ȯ ������ üũ
                    if (timers[tmpStage][i] < 0)
                    {
                        Debug.Log($"Spawn {spawnElements[tmpStage][i].prefab.name}");
                        GameObject go = Instantiate(spawnElements[tmpStage][i].prefab,
                                    WayPoints.instance.GetFirstWayPoint().position,
                                    Quaternion.identity);
                        Debug.Log(go);
                        counts[tmpStage][i]--;
                        timers[tmpStage][i] = spawnElements[tmpStage][i].delay;
                    }
                    else
                        timers[tmpStage][i] -= Time.deltaTime;

                }
            }
            yield return null;
        }        
    }


}
