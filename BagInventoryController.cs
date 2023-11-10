using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 
 * BagInventoryController used to manipulate pre-raid slots utilizing parent SlotPanelController class
 * 
 * Unique Behaviour: Populates and Updates via Ledger
 * 
 */
public class BagInventoryController : SlotPanelController
{

    [SerializeField]
    private InventoryScriptableObject bagScriptableObject;

    private void OnEnable() //NOTE: Formerly OnEnable
    {

        if (bagScriptableObject)
            PopulateByLedger(bagScriptableObject);
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        if (bagScriptableObject)
            UpdateByLedger(ref bagScriptableObject); //Takes slots and uses that info to populate ledger via reference (so direct access to change the scriptableObject)
    }

}
