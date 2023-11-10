using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;

/*
 * 
 * SlotPanelController: Collects list of available slots, and contains helper functions to populate and retrieve information about the slots...
 * Children/extensions of SlotPanelController contain variables and functions in charge of storing the underlying data
 * LootController - populates based on runtime list, stores to list onDisable
 * BagController - populated based on ledger list at start of scene, only stores upon successful level completion
 * 
 */

public class SlotPanelController : MonoBehaviour
{

    [SerializeField]
    protected List<SlotController> slots;

    private int minSlotCount;

    //public UnityEvent OnMenuClose;

    protected virtual void Awake()
    {
        slots = new List<SlotController>(); //create, pre-clear, and subsequently store all valid SlotControllers parented under this script's gameObject

        slots.Clear();

        foreach (SlotController selects in GetComponentsInChildren<SlotController>(true))
        {
            slots.Add(selects);
            //Debug.Log("Slots Added");
        }
    }

    //OnEnable
    //Start

    protected virtual void OnDisable() {
        //OnMenuClose.Invoke();
    }

    protected void PopulateByLedger(InventoryScriptableObject iso)
    {
        minSlotCount = Mathf.Min(slots.Count, iso.items.Count);
        //NOTE: the minCount is set once upon enabling, meaning must be set before you can actually interact and change the ledger. fyi in case u ever try and manipulate ledger without enabling
        for (int i = 0; i < minSlotCount; i++)
            slots[i].PlaceItem(iso.items[i]);
    }

    //updates the ledger based on the slots
    protected void UpdateByLedger(ref InventoryScriptableObject iso)
    {
        minSlotCount = Mathf.Min(slots.Count, iso.items.Count);

        for (int i = 0; i < minSlotCount; i++)
            iso.items[i] = slots[i].GetItem();
    }

    protected void UpdateLegderviaReference(List<Item> items, ref InventoryScriptableObject iso) {
        minSlotCount = Mathf.Min(items.Count, iso.items.Count);

        for (int i = 0; i < minSlotCount; i++)
            iso.items[i] = items[i];
    }


    //Fill by reference: items list can have null
    //Note: the number of items storable in a lootpanel will, due to disabling, always equal the same amount of items generated
    //also note that this goes through all the slots
    //but items may not fill the remaing
    protected void PopulateByReference(Lootable loot) {


        List<Item> items = loot.m_loot;

        for (int i = 0; i < slots.Count; i++) {
            //go through each slot
            if (i < loot.boxSize)
            {
                //i is under boxSize range, do not disable slot
                //enable the slot, place an item (even if null)
                slots[i].EnableSlot();
                slots[i].PlaceItem(items[i]);
            }
            else
            {
                //once here, the index is out of range of possible items, so just disable the slots
                slots[i].ResetSlot();
                slots[i].DisableSlot();

            }
        }

    }

    //Go through slots and pass that info over to the references item list
    protected void UpdateByReference(ref List<Item> items) {
        
        for (int i = 0; i < items.Count; i++)
        {
            if (i < slots.Count)
                items[i] = slots[i].GetItem();
            else
                items[i] = null;
        }
    }

    protected void DisableAllSlots() {
        //Debug.Log("Slots: " + slots.Count);
        for (int i = 0; i < slots.Count; i++) {
            slots[i].DisableSlot();
        }
    }

    public void SelectNextAvailableSlot() {

        if (slots.Count <= 0)
            return;

        for (int i = 0; i < slots.Count; i++)
        {
            if (!slots[i].filled) {
                slots[i].SelectSlot();
                return;
            }
                
        }

        slots[0].SelectSlot();
    }

    public void SelectFirstSlot() {
        if(slots.Count > 1)
            slots[0].SelectSlot();
    
    }

    public bool PlaceAtNextAvailableSlot(Item place) {

        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].PlaceItem(place))
            {
                return true;
            }
        }

        return false;
    }

    protected void SlotSetter(ref List<SlotController> slotList) {
        if (slotList == null) {
            slotList = new List<SlotController>();

            slotList.Clear();

            foreach (SlotController selects in GetComponentsInChildren<SlotController>(true))
            {
                slotList.Add(selects);
            }
        }
    }

}
