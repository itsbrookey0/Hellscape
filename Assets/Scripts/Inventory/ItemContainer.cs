using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemContainer : MonoBehaviour, IItemContainer
{

    [SerializeField] protected ItemSlot[] itemSlots;
    public List<GenericItem> genericItems = new List<GenericItem>();

    public virtual bool CanAddItem(GenericItem item, int amount = 1)
    {
        int freeSpaces = 0;

        foreach (ItemSlot itemSlot in itemSlots)
        {
            if(itemSlot.Item == null || itemSlot.Item.ID == item.ID)
            {
                freeSpaces += item.MaximumStacks - itemSlot.Amount;
            }
        }

        return freeSpaces >= amount;
    }



    public virtual bool AddItem(GenericItem item)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].Item == null || (itemSlots[i].Item.ID == item.ID && itemSlots[i].Amount < item.MaximumStacks))
            {
                itemSlots[i].Item = item;
                genericItems.Add(item);
                itemSlots[i].Amount++;
                return true;
            }
        }

        return false;
    }

    public virtual bool RemoveItem(GenericItem item)
    {

        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].Item == item)
            {
                itemSlots[i].Amount--;
                genericItems.Remove(item);
                if (itemSlots[i].Amount == 0)
                {
                    itemSlots[i].Item = null;
                    genericItems.Remove(item);
                }

                return true;
            }
        }

        return false;

    }

    public virtual GenericItem RemoveItem(string itemID)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            GenericItem item = itemSlots[i].Item;
            if (item != null && item.ID == itemID)
            {
                itemSlots[i].Amount--;
                if (itemSlots[i].Amount == 0)
                {
                    itemSlots[i].Item = null;
                    
                }
                return item;
            }
        }
        return null;
    }


    public virtual bool ContainsItem(GenericItem item)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].Item == item)
            {
                return true;
            }

        }
        return false;
    }

    public virtual int ItemCount(string itemID)
    {
        int number = 0;
        for (int i = 0; i < itemSlots.Length; i++)
        {
            GenericItem item = itemSlots[i].Item;
            if (item != null && item.ID == itemID)
            {
                number++;
            }

        }
        return number;
    }


}
