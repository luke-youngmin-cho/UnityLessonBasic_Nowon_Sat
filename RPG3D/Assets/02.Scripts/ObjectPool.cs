using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ǯ�� ��Ҹ� ����ϰ��� �� ��ҵ��� size ��ŭ ��ü�� ���� �����ϰ� ��Ȱ��ȭ
// ��ü�� �ʿ��ϸ� �ش� ��ü�� Ȱ��ȭ�ϰ� Ǯ ������ ����
// ���� ��ȯ�� ��ü�� ��� ��� ���� ��� ���� ��ü�� Instantiate �ؼ� Ǯ�� ����ϰ� ��ȯ��. 
public class ObjectPool : MonoBehaviour
{
    private static ObjectPool _instance;
    public static ObjectPool instance
    {
        get
        {
            if (_instance == null)
                _instance = Instantiate(Resources.Load<ObjectPool>("ObjectPool"));
            return _instance;
        }
    }
    public static bool isReady;
    List<PoolElement> poolElements = new List<PoolElement>();
    List<GameObject> spawnedObjects = new List<GameObject>();
    Dictionary<string, Queue<GameObject>> spawnedQueueDictionary = new Dictionary<string, Queue<GameObject>>();

    // Ǯ�� ����ϴ� �Լ�
    public void AddPoolElement(PoolElement poolElement)
    {
        poolElements.Add(poolElement);
    }

    // Ǯ ��ҵ��� �����ϴ� �Լ�
    public void CreatePoolElements()
    {
        foreach (PoolElement poolElement in poolElements)
        {
            spawnedQueueDictionary.Add(poolElement.tag, new Queue<GameObject>());
            for (int i = 0; i < poolElement.size; i++)
            {
                GameObject obj = CreateNewObject(poolElement.tag, poolElement.prefab);
                spawnedQueueDictionary[poolElement.tag].Enqueue(obj);
            }
        }
        isReady = true;
    }

    // ��ü ��ȯ �Լ�
    public static GameObject SpawnFromPool(string tag, Vector3 position)
        => instance.Spawn(tag, position);

    // Ǯ�� �ǵ����� �Լ�
    public static void ReturnToPool(GameObject obj)
    {
        // �ش� ������Ʈ�� Ǯ�� ��ϵǾ��ִ��� üũ
        if (instance.spawnedQueueDictionary.TryGetValue(obj.name, out Queue<GameObject> queue))
        {
            obj.transform.position = instance.transform.position;
            queue.Enqueue(obj);
        }
        else
            throw new System.Exception($"{obj.name} wasn't registered");
    }

    // ��ȯ�� Ư�� �±��� ��ü ���� ��ȯ�ϴ� �Լ�
    public static int GetSpawnedObjectNumber(string tag)
    {
        int count = 0;
        foreach (var go in instance.spawnedObjects)
        {
            if (go.name == tag &&
                go.activeSelf)
                count++;
        }
        return count;
    }

    //======================================================================
    //************************* Private Methods ****************************
    //======================================================================

    private void Awake()
    {
        if (_instance != null)
        {
            for (int i = 0; i < spawnedObjects.Count; i++)
                Destroy(spawnedObjects[i]);
            spawnedObjects.Clear();
            foreach (var sub in spawnedQueueDictionary)
                sub.Value.Clear();
            spawnedQueueDictionary.Clear();
            Destroy(_instance);
            _instance = null;
            System.GC.Collect();
            _instance = instance;
        }
    }

    // ��ü �����Լ�
    private GameObject CreateNewObject(string tag, GameObject prefab)
    {
        GameObject obj = Instantiate(prefab, transform);
        obj.name = tag;
        obj.SetActive(false);
        ArrangePool(obj);
        return obj;
    }

    // ��ȯ�ϴ� �Լ�
    private GameObject Spawn(string tag, Vector3 position)
    {
        // ��ȯ�ϰ���� �±װ� ������ �ִ��� üũ
        if (spawnedQueueDictionary.TryGetValue(tag, out Queue<GameObject> queue))
        {
            // �ش� �±��� ��ü���� �̹� ��� ���ǰ� �ִٸ� ���� ����
            if (queue.Count == 0)
            {
                PoolElement poolElement = poolElements.Find(x => x.tag == tag);
                CreateNewObject(poolElement.tag, poolElement.prefab);
            }
        
            GameObject objectToSpawn = queue.Dequeue();
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = Quaternion.identity;
            objectToSpawn.SetActive(true);
        
            return objectToSpawn;
        }
        else
            throw new System.Exception($"Pool doesn't contains {tag}");
    }


    // Ǯ�� �����ϴ� �Լ�
    private void ArrangePool(GameObject obj)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == obj.name)
            {
                obj.transform.SetSiblingIndex(i);
                spawnedObjects.Insert(i, obj);
                break;
            }
        }
    }
}

// Ǯ�� ����� ��� Ÿ��(Ŭ����)
[System.Serializable]
public class PoolElement
{
    public string tag;
    public GameObject prefab;
    public int size;
}