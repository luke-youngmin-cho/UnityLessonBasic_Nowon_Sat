using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController_HPPosion : ItemControllerUsable
{
    public override void Use()
    {
        Debug.Log("�÷��̾��� ü���� ȸ���Ǿ���!");
        Item removingItem = Instantiate(item);
        removingItem.num = 1;
        InventoryModel.instance.RemoveItemData(removingItem);
    }
}
