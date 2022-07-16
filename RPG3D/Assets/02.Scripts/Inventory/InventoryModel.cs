using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryModel
{
    private static InventoryModel _instance;
    public static InventoryModel instance
    {
        get
        {
            if (_instance == null)
                _instance = new InventoryModel();
            return _instance;
        }
    }

    [System.Serializable]
    public class InventoryData
    {
        public List<InventoryItemData> equipmentItemsData;
        public List<InventoryItemData> spendItemsData;
        public List<InventoryItemData> etcItemsData;

        public InventoryData()
        {
            equipmentItemsData = new List<InventoryItemData>();
            spendItemsData = new List<InventoryItemData>();
            etcItemsData = new List<InventoryItemData>();
        }
    }

    private InventoryData _inventoryData;
    private string _dirPath;
    private List<Item> _equipmentItems;
    private List<Item> _spendItems;
    private List<Item> _etcItems;


    public InventoryModel()
    {
        _inventoryData = new InventoryData();
        _dirPath = $"{Application.persistentDataPath}/InventoryData";
        _equipmentItems = new List<Item>();
        _spendItems = new List<Item>();
        _etcItems = new List<Item>();
    }

    public void AddItemData(Item item)
    {
        List<InventoryItemData> tmpDataList = null;
        List<Item> tmpItems = null;
        switch (item.type)
        {
            case ItemType.Equipment:
                tmpDataList = _inventoryData.equipmentItemsData;
                tmpItems = _equipmentItems;
                break;
            case ItemType.Spend:
                tmpDataList = _inventoryData.spendItemsData;
                tmpItems = _spendItems;
                break;
            case ItemType.ETC:
                tmpDataList = _inventoryData.etcItemsData;
                tmpItems = _etcItems;
                break;
            default:
                throw new System.Exception("InventoryModel.AddItemData() : �߸��� ������ Ÿ��");
        }

        InventoryItemData oldData = tmpDataList.Find(x => x.itemTag == item.tag);
        Item oldItem = tmpItems.Find(x => x.tag == item.tag);

        if (oldData.Equals(default(InventoryItemData)))
        {
            tmpDataList.Add(new InventoryItemData()
            {
                itemType = item.type,
                itemTag = item.tag,
                num = item.num,
            });
            Item newItem = GameObject.Instantiate(item);
            tmpItems.Add(newItem);
        }
        else
        {
            tmpDataList.Add(new InventoryItemData()
            {
                itemType = item.type,
                itemTag = item.tag,
                num = item.num + oldData.num,
            });
            tmpDataList.Remove(oldData);
            Item newItem = GameObject.Instantiate(item);
            newItem.num = item.num + oldData.num;
            tmpItems.Add(newItem);
            tmpItems.Remove(oldItem);
        }

        InventoryView.instance.Refresh(item.type, tmpItems);
        SaveInventoryDataFile();
    }

    public void RemoveItemData(Item item)
    {
        List<InventoryItemData> tmpDataList = null;
        List<Item> tmpItems = null;
        switch (item.type)
        {
            case ItemType.Equipment:
                tmpDataList = _inventoryData.equipmentItemsData;
                tmpItems = _equipmentItems;
                break;
            case ItemType.Spend:
                tmpDataList = _inventoryData.spendItemsData;
                tmpItems = _spendItems;
                break;
            case ItemType.ETC:
                tmpDataList = _inventoryData.etcItemsData;
                tmpItems = _etcItems;
                break;
            default:
                throw new System.Exception("InventoryModel.RemoveItemData() : �߸��� ������ Ÿ��");
        }

        InventoryItemData oldData = tmpDataList.Find(x => x.itemTag == item.tag);
        Item oldItem = tmpItems.Find(x => x.tag == item.tag);

        if (oldData.Equals(default(InventoryItemData)))
        {
            throw new System.Exception("�������� ���µ� ������� �õ��� ����");
        }
        else
        {
            int newNum = oldData.num - item.num;
            if (newNum < 0)
            {
                throw new System.Exception("�����ϴ� ������ �������� �� ���� ������� �õ���");
            }   
            else if (newNum > 0)
            {
                tmpDataList.Add(new InventoryItemData()
                {
                    itemType = item.type,
                    itemTag = item.tag,
                    num = oldData.num - item.num,
                });
                Item newItem = GameObject.Instantiate(item);
                newItem.num = oldData.num - item.num;
                tmpItems.Add(newItem);
            }

            tmpDataList.Remove(oldData);
            tmpItems.Remove(oldItem);
        }

        InventoryView.instance.Refresh(item.type, tmpItems);
        SaveInventoryDataFile();
    }

    public void SaveInventoryDataFile()
    {
        if (System.IO.Directory.Exists(_dirPath) == false)
            System.IO.Directory.CreateDirectory(_dirPath);

        string jsonPath = _dirPath + $"/InventoryData.json";
        string jsonData = JsonUtility.ToJson(_inventoryData);
        System.IO.File.WriteAllText(jsonPath, jsonData);
    }

    public void LoadInventoryDataFile()
    {
        string jsonPath = _dirPath + $"/InventoryData.json";
        if (System.IO.Directory.Exists(_dirPath))
        {
            string jsonData = System.IO.File.ReadAllText(jsonPath);
            _inventoryData = JsonUtility.FromJson<InventoryData>(jsonData);
        }
       //else
       //    throw new System.Exception("�κ��丮 �����͸� �ε��� �� �����ϴ�. ���ϰ�� ����");
    }

    public void RefreshAllItems()
    {
        Item tmp = null;
        foreach (var itemData in _inventoryData.equipmentItemsData)
        {
            if (ItemAssets.instance.TryGetItem(itemData, out tmp))
            {
                tmp = GameObject.Instantiate(tmp);
                tmp.num = itemData.num;
                _equipmentItems.Add(tmp);
            }
        }
        foreach (var itemData in _inventoryData.spendItemsData)
        {
            if (ItemAssets.instance.TryGetItem(itemData, out tmp))
            {
                tmp = GameObject.Instantiate(tmp);
                tmp.num = itemData.num;
                _spendItems.Add(tmp);
            }
        }
        foreach (var itemData in _inventoryData.etcItemsData)
        {
            if (ItemAssets.instance.TryGetItem(itemData, out tmp))
            {
                tmp = GameObject.Instantiate(tmp);
                tmp.num = itemData.num;
                _etcItems.Add(tmp);
            }
        }
        InventoryView.instance.Refresh(ItemType.Equipment, _equipmentItems);
        InventoryView.instance.Refresh(ItemType.Spend, _spendItems);
        InventoryView.instance.Refresh(ItemType.ETC, _etcItems);
    }
}
