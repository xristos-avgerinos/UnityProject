﻿using UnityEngine;

namespace Invector.vItemManager
{
    [vClassHeader("Contains Item Trigger", "Simple trigger to check if the Player has a specific Item, you can also use Events to trigger something in case you have the item.", openClose = false)]
    public class vContainsItemTrigger : vMonoBehaviour
    {
        public bool getItemByName;
        [vHideInInspector("getItemByName")]
        public string itemName;
        [vHideInInspector("getItemByName", true)]
        public int itemID;
        public bool useTriggerStay;
        public int desiredAmount=1;
        [Header("OnTriggerEnter/Stay")]
        public UnityEngine.Events.UnityEvent onContains;
        public UnityEngine.Events.UnityEvent onNotContains;
        [Header("OnTriggerExit")]
        public UnityEngine.Events.UnityEvent onExit;

        public vItemManager itemManager;
        public void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                var itemManager = other.GetComponent<vItemManager>();
                if (itemManager)
                {                    
                    CheckItem(itemManager);
                }
            }
        }

        public void RemoveDesiredItem()
        {
            if(itemManager)
            {
                if (getItemByName)
                {
                    // VERIFY IF YOU HAVE A SPECIFIC ITEM IN YOUR INVENTORY
                    if (ContainsItem(itemManager))
                    {
                        itemManager.DestroyItem(itemManager.GetItem(itemName),desiredAmount>1?desiredAmount: 1);
                    }
                }
                else
                {
                    // VERIFY IF YOU HAVE A SPECIFIC ITEM IN YOUR INVENTORY
                    if (ContainsItem(itemManager))
                    {
                        itemManager.DestroyItem(itemManager.GetItem(itemID), desiredAmount > 1 ? desiredAmount : 1);
                    }
                }
            }
        }

        public void OnTriggerStay(Collider other)
        {
            if (!useTriggerStay) return;
            if (other.gameObject.CompareTag("Player"))
            {
                itemManager = other.GetComponent<vItemManager>();
                if (itemManager)
                {
                    CheckItem(itemManager);
                }
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                onExit.Invoke();
            }
        }
       
        protected virtual void CheckItem(vItemManager itemManager)
        {
            if (getItemByName)
            {
                // VERIFY IF YOU HAVE A SPECIFIC ITEM IN YOUR INVENTORY
                if (ContainsItem(itemManager))
                {
                  
                    // AUTOMATICALLY EQUIP THE ITEM IF YOU HAVE IT
                    //
                    // - vItem can be access using the itemManager.GetItem(itemName)
                    // - indexArea is the equipmentArea that you want to equip the item, for example 0 is RightArm, 1 is LeftArm, 2 is Consumable.
                    // - Immediate is true to instantly equip the weapon or false to trigger the equip animation.
                    //itemManager.AutoEquipItem(itemManager.GetItem(itemName), 0, false);
                    // trigger OnContains Event
                    onContains.Invoke();
                }
                else
                    onNotContains.Invoke();
            }
            else
            {
                // VERIFY IF YOU HAVE A SPECIFIC ITEM IN YOUR INVENTORY
                if (ContainsItem(itemManager))
                {
                    // AUTOMATICALLY EQUIP THE ITEM IF YOU HAVE IT
                    //
                    // - vItem can be access using the itemManager.GetItem(itemName)
                    // - indexArea is the equipmentArea that you want to equip the item, for example 0 is RightArm, 1 is LeftArm, 2 is Consumable.
                    // - Immediate is true to instantly equip the weapon or false to trigger the equip animation.
                    //itemManager.AutoEquipItem(itemManager.GetItem(itemID), 0, false);
                    // trigger OnContains Event
                    onContains.Invoke();
                }
                else
                    onNotContains.Invoke();
            }
        }

        protected bool ContainsItem(vItemManager itemManager)
        {
            return desiredAmount > 1 ? (getItemByName ? itemManager.ContainItem(itemName, desiredAmount) : itemManager.ContainItem(itemID, desiredAmount)) :
                   (getItemByName ? itemManager.ContainItem(itemName) : itemManager.ContainItem(itemID));
        }
    }

}
