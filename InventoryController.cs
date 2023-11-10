using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

/*
 * 
 * SlotPanelControllers have reference to all child slots, and comes with helper functions to enable, disable, and initialize buttons
 * Unique behavior of Inventory Controller is that it populates and updates Inventory slots via ledger (scriptable object with on-disk data of inventory/stash)
 *
 * Start - Stores slots
 * OnEnable - Pulls data from disk
 * OnDisable - Updates ledger/disk
 * 
 */

public class InventoryController : SlotPanelController
{

    [SerializeField]
    private InventoryScriptableObject inventoryScriptableObject;

    private void OnEnable()
    {
        
        if (inventoryScriptableObject)
            PopulateByLedger(inventoryScriptableObject); //populates inventory slots pulling from ledger

        //Note: The PopulateByLedger assumes all buttons are enabled and places them, leaving them null/disabled when inventoryScript passes in a null/empty item
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        if (inventoryScriptableObject)
            UpdateByLedger(ref inventoryScriptableObject);
    }

}
