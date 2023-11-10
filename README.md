# Inventory System
Above are the 5 core scripts used to manage inventory slots in an extraction/scavenging game.

[To see these scripts in action, click here](https://www.youtube.com/watch?v=IrrSBgEFJQk)

[Check out my complete portfolio here](https://portfolium.com/juliodev/portfolio)

**SlotPanelController** is the main parent class of which the remaining 4 scripts...

**BagInventoryController** <br />
**InventoryController** <br />
**BagRaidController** and <br />
**LootController**

...inherit from to utilize basic slot initialization and helper funtions.

As stated, the **SlotPanelController's** primary focus is gathering valid **SlotController** components in its children.

**SlotControllers** are scripts that, when attached to UI Buttons, give the button properties of an inventory slot. These properties include menu/controller navigation capability, button marking (i.e. filled, available, disabled), and the ability to move the item onto other open slots.

Along with functions that return values such as nearest available slot, **SlotPanelController** also contains important scriptableObjects used to populate slots with items.

The functions can be split into two categories:

_Data via Ledger_ or _Data via Reference_

When populating via _ledger_, slot data is pulled directly from a scriptableObject that stores the state of the slots and inventory (this data exists outside of runtime and is considered 'saved to disk'). When populating by _reference_, we only use instance copies of the _ledger_ - data that, once copied, only exists during runtime. This allows us to manipulate copies of the disk data without interfering with the data on disk.

With these core functions outlined, the functionality can be used in unique ways by the 4 children classes of SlotPanelController. In short:

**BagInventoryController** - populates via _ledger_. Any changes to the bag slots in this UI Menu is immediately reflected to the _ledger_/on-disk data. This script only runs in the Main Menu so players can see what underlying items they have in their bag.

**InventoryController** - like BagInventoryController, this script is used in the Main Menu to display the on-disk slot data but for their inventory instead of the bag. Note: Bag Slots are items you can take to the main gameplay scene. Inventory Slots only exist in the Main Menu and is used to show what items you can put into your bag which are then subsequently taken into the gameplay scene.

**BagRaidController** - similar to BagInventoryController but this script only populates via _ledger_ ONCE upon start of the gameplay scene. Any changes there on will NOT be reflected to the _ledger_ until the level is completed. Think of this as losing your progress until the level is complete.

**LootController** - paired with BagRaidController, this script controls the population of lootables in the game. This script is only meant to display in-game lootable slots whose items can put into the BagRaid version of your bag. This data technically is a pull by _reference_, wherein the reference is the slot/item data of a lootable crate.

Together, these scripts allow you to scavenge the world for items - storing them in your stash after every successful level. Only when in the Main Menu can you manipulate the on-disk data, creating a safety net so your 'saved' data is only touched in a safe environment.
