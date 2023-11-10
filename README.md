# Inventory System
Above are the 5 core scripts used to manage inventory slots in an extraction/scavenging game.

**SlotPanelController** is the main parent class of which the remaining 4 scripts...

**BagInventoryController**
**InventoryController**
**BagRaidController** and
**LootController**

...inherit from to utilize basic slot initialization and helper funtions.

As stated, the **SlotPanelController** script's primary focus is looking for and gathering valid components in its children that contain **SlotController** scripts.

**SlotControllers** are scripts that, when attached to UI Buttons, give the button the properties of an inventory slot. These properties include menu/controller navigation capability, marking the button as filled (including a unique image sprite to identify what item is currently in that slot), and the ability to move the item onto other slots.

Along with functions that find the nearest available slot, disable slots, and place items at designated slots - **SlotPanelController** also contains very important data fields used to populate slots with items in the player's inventory. These core functions can be split into two categories: Data via Ledger or Data via Reference. When populating via ledger, we pull slot data directly from a scriptableObject that is used as on-disk data (this data exists outside of runtime). When populating by reference, we only use instance copies of the ledger to populate slots (this data only exists during runtime and is used at key moments to copy over to the ledger). This allows us to manipulate copies of the disk data without touching the underlying data.

With these core functions outlined, the functionality can be used in unique ways by the 4 children classes of SlotPanelController. In short:

**BagInventoryController** - populates via ledger. Any changes to the bag slots in this UI Menu is immediately reflected to the ledger/on-disk data. This script only runs in the Main Menu so players can see what underlying items they have in their bag.

**InventoryController** - like BagInventoryController, this script is used in the Main Menu to display the on-disk slot data but for their inventory instead of the bag. Note: Bag Slots are items you can take to the main gameplay scene. Inventory Slots only exist in the Main Menu and is used to show what items you can put into your bag which are then subsequently taken into the gameplay scene.

**BagRaidController** - similar to BagInventoryController but this script only populates via ledger ONCE upon start of the gameplay scene. Any changes there on will NOT be reflected to the ledger until the level is completed. Think of this as losing your progress until the level is complete.

**LootController** - paired with BagRaidController, this script controls the population of lootables in the game. This script is only meant to display in-game lootable slots whose items can put into the BagRaid version of your bag. This data technically is a pull by reference, wherein the reference is the slot/item data of a lootable crate.

Together, these scripts allow you to scavenge the world for items - storing them in your stash after every successful level. Only when in the Main Menu can you manipulate the on-disk data, creating a safety net so your 'saved' data is only touched in a safe environment.
