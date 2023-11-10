using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

/*
 * 
 * LootController: Controls loot slots during raid
 * 
 * Unique behavior: Only populates via Lootable currentLoot, which gets initialized via LootableItems in the field
 * When no lootable exists, slots get disabled until a new lootTarget populates the slots
 * 
 */

public class LootController : SlotPanelController
{

    [SerializeReference]
    private Lootable currentLoot;

    protected override void Awake()
    {
        base.Awake();

        //suppose it's possible that currentLoot is pre-set before Awake
        //if so, do nothing
        if (!currentLoot)
            currentLoot = null; //?? this the only to not erase current loot for the first-frame that it becomes active?
    }

    public void SetReference(Lootable lootable)
    {
        //Debug.Log("Reference Set: Lootable " + lootable.name);
        this.currentLoot = lootable;
    }


    //Called everytime the menu is opened, pull from the static
    private void OnEnable()
    {

        if (currentLoot)
        {
            //Debug.Log("Populating with Loot");
            PopulateByReference(currentLoot);
            currentLoot.OpenLoot();
        }
        else {
            //Debug.Log("No Loot: Disabling Slots");
            DisableAllSlots();
        }

        #region Notes-On-Loot-Controller
        //So the thing with this script is that it does a lot of it's initialization in Awake and OnEnable
        //Problem with that, is that these elements don't actually get populated until the gameobject becomes active (almost like the components get pre-filled and only actually populated OnEnable)
        //Regardless, this caused a weird behaviour wherein the first time you Enable this panel, none of the pre-filled components got activated
        //Or rather, the components got overwritten the very first time you open the panel

        //For example, triggering a lootable set the currentLoot value to that interactable. The first time you open the raiditempanel, being the first time, erased the reference since the reference was set before the raidpanel became active
        //The solution? Let the raid panel be active at the start of the game, and on the first frame after the initial start, the panel becomes inactive and ready for the raid

        //PROBLEM?? Is that this also meant the bagraidcontroller loaded at the start, and would populate itself using the ledger. This would load with the untouched fresh version of the bag, even though pre-raid you can change the
        //order of your bag items.

        //Solution? Let RaidPanel be active at the start of the game, let it become inactive via MenuController and then delegate the responsibilty of loading the bag to SceneManager
        //SceneManager checks to see if the next scene is the GameScene and if so, will load the bag

	//Final solution: GameControlLink script updates raid bag during Start. This GameControlLink exists outside of singleton and runs Start once upon starting Main gameplay scene. By calling to the singleton GameController, the
	//GameControl Link acts as a soft Start function for our singleton
        #endregion Notes-On-Loot-Controller
    }

    //Start

    private void Start()
    {
        //SlotSetter(ref this.slots);
    }

    //essentially called twice: manually closing the menu or exiting trigger
    //closing the menu can't release the reference - only trigger
    protected override void OnDisable()
    {
        base.OnDisable();
        
        //if a lootable was passed in, update it's slots and close the box
        if (currentLoot) {
            UpdateByReference(ref currentLoot.m_loot);
            currentLoot.CloseLoot();
        }

        //reset all the slots too
        DisableAllSlots();
    }

    //this is for exiting trigger
    //close box, release reference so that next time it's open a new list (or no list) can come in
    public void ReleaseReference() {
        if (currentLoot)
            this.currentLoot.CloseLoot();

        this.currentLoot = null;
    }

}
