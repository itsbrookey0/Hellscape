using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ItemAmount
{
    public GenericItem Item;
    public int Amount;
}

[CreateAssetMenu]
public class CraftingRecipe : ScriptableObject
{
    public List<ItemAmount> Materials;
    public List<ItemAmount> Results;

    public bool CanCraft(IItemContainer itemContainer)
    {
        return HasMaterials(itemContainer) && HasSpace(itemContainer);
    }

    public bool HasMaterials(IItemContainer itemContainer)
    {
        foreach (ItemAmount itemAmount in Materials)
        {
            if(itemContainer.ItemCount(itemAmount.Item.ID) < itemAmount.Amount)
            {
                Debug.Log("Can't craft method goes here.");
                return false;
            }
        }
        return true;
    }

    public bool HasSpace(IItemContainer itemContainer)
    {
        foreach (ItemAmount itemAmount in Results)
        {
            if(!itemContainer.CanAddItem(itemAmount.Item, itemAmount.Amount))
            {
                return false;
            }
        }
        return true;
    }

    public void Craft(IItemContainer itemContainer)
    {
        if(CanCraft(itemContainer))
        {
            RemoveMaterials(itemContainer);
            AddResults(itemContainer);
        }
    }

    private void RemoveMaterials(IItemContainer itemContainer)
    {
            foreach (ItemAmount itemAmount in Materials)
            {
                for (int i = 0; i < itemAmount.Amount; i++)
                {
                    GenericItem oldItem = itemContainer.RemoveItem(itemAmount.Item.ID);
                    oldItem.Destroy();
                }
            }

    }

    private void AddResults(IItemContainer itemContainer)
    {
        foreach (ItemAmount itemAmount in Results)
        {
            for (int i = 0; i < itemAmount.Amount; i++)
            {
                itemContainer.AddItem(itemAmount.Item.GetCopy());
                FindObjectOfType<AudioManager>().Play("Craft"); //CRAFTING ITEM SOUND
            }
        }
    }
}

