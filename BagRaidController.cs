using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine;

/*
 * 
 * BagRaidController: Controls bag slots during raid
 * 
 * Unique behavior: Only populates slots once on Start, does not touch ledger until post-raid (PostRaidLedgerUpdate called on death/extraction)
 * 
 */

public class BagRaidController : SlotPanelController
{

    [SerializeField]
    private InventoryScriptableObject bagScriptableObject;
    //populates slots once, not every time it opens via OnEnable

    [SerializeField]
    private List<SlotController> throwables;

    private int activeThrowable;

    public delegate void UpdateActiveItem();
    public UpdateActiveItem updateActiveItem;

    public delegate void UseThrowable(Item item);
    public UseThrowable useThrowable;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        SceneManager.activeSceneChanged += ChangedActiveScene;

        if (bagScriptableObject)
            PopulateByLedger(bagScriptableObject);


        throwables = new List<SlotController>();

        //go through slots, store a reference to the 
        //PopulateThrowables();
    }

    private void OnEnable()
    {
        
    }


    protected override void OnDisable()
    {
        base.OnDisable();

        //maybe here is where it should update throwables and stuff
        //defiunitely makes sense to have a seperate list of throwables that pulls the grenades from the UI as references
        //basically, you should be able to disable a grenade from your inventory by disabling it's reference in the list
        //populate list...
        PopulateThrowables();

        updateActiveItem?.Invoke();
    }

    private void Update()
    {
        //oh ya doesnt get called when inactive...
        //ThrowableCheck();
    }

    public void ThrowableCheck() {
        if (throwables != null)
        {
            //so a throwable exists...

            if ((activeThrowable >= 0) && (activeThrowable < throwables.Count))
            {
                //active throwable is in range...

                //useThrowable
                useThrowable?.Invoke(throwables[activeThrowable].GetItem());

                //remove the item from the bag..
                throwables[activeThrowable].ResetSlot();

                throwables.RemoveAt(activeThrowable);

                //set active throwable to next active throwable...
                //if you are on index 2, set it to 3
                //or 0?
                activeThrowable = 0;

                updateActiveItem?.Invoke();

            }

            //reset the slot in throwables...
            //remove this element from the list..

        }
    }

    private void ChangedActiveScene(Scene current, Scene next)
    {

        Debug.Log(current.name);

        //TODO: Proper bag loading in raid...

        //SO WHAT HAPPENS PUSSSY
        //i guess the big thing is when to enable BagRaid
        //if it get's enabled at Start, then loot panel works but bag loads from ledger too early
        //if its enabled during raid, bag works but loot panel breaks for the first frame since i guess it refreshes any references OnEnable

        //if (next.name.Equals("GameScene"))
        //    PreRaidLedgerPopulate();

        //Issue: Ledger does not get updated until AFTER the next scene is loaded...
        //I'm assuming it's because this method is fired BEFORE the scene loads, thus BEFORE the stashPanel actually closes and is updated
        //Good workaround is to close the raid menu by having an "Are You Sure" panel appear before actually entering a raid...
        //Idk...

        #region Why-ChangedActiveScene-Populated-BagRaid-SlotPanelController
        //Some things to note: SlotPanelController has access to the UI panel's slots - slots being UI buttons with SlotPanel scripts that hold references to items as well as slots' enabled states
        //That being said, SlotPanelController (and BagRaidController - a child of SPC) can disabled buttons, make them un-interactable, and sets their items etc.

        //SPC also loads items into the UI button slots via the list of slotpanel scripts attached to each button. Where and how are special for each inventory type
        //The Bag SlotPanelControllers are broken into two groups: pre-raid and mid-raid. Pre-raid, the bag is directly loaded via disk version of bag (saved and stored on disk)
        //                                                                                Mid-raid, bag is directly loaded via disk but only once during the start of the scene

        //That is why the ChangedActiveScene call here populates the bag. This function is called once upon changing scenes, and can load the bag once during the start of the raid scene
        //This gameobject, BagRaidController, exists as a child of GameController and GameController is a DoNotDestoryScript
        //Since GameController only has a single Start call and never again, so does BagRaidController. The way around this is to load during Scene Changes
        //Otherwise, BagRaid would load once at the Start of the entire game and it would pull from ledger (which hasn't even been updated by the player) and you'd load into the world with a unsync version of 'bag'
        #endregion Why-ChangedActiveScene-Populated-BagRaid-SlotPanelController
    }

    public void PreRaidLedgerPopulate() {
        if (bagScriptableObject)
            PopulateByLedger(bagScriptableObject);
    }

    public void PostRaidLedgerUpdate() {
        //UpdateLegderviaReference(bagItems, ref bagScriptableObject);
        UpdateByLedger(ref bagScriptableObject);
    }

    public void PopulateThrowables() {

        //clear throwables each time, simplest way to ensure no duplicate items
        throwables.Clear();

        foreach (SlotController slot in slots) {
            Item item = slot.GetItem();

            if (item) {
                if (item.itemType == ItemType.throwable)
                    throwables.Add(slot);
            }
            
        }
    }

    public SlotController GetActiveThrowable() {

        if (throwables == null)
            return null;
        
        if ((throwables.Count > 0) && (activeThrowable <= (throwables.Count - 1)))
            return throwables[activeThrowable];
        else
            return null;
    }

    //delete the first throwable
    public void DeleteThrowable() {
        if (throwables.Count > 0)
            throwables[0].ResetSlot();
    }
}
