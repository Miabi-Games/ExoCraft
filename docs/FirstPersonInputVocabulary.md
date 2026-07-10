# First Person Input Vocabulary

This document defines a shared input vocabulary for ExoCraft and related future games. Individual games may expose these actions differently depending on their primary interface. Some games may emphasize direct first-person or chase-camera control, while others may treat manual movement as an optional advanced layer beneath a primarily point-and-click or command-driven interface.

*This is not the plan for ExoCraft: ExoCraft will have a different control scheme, and this document is not intended to be a guide for that game. ExoCraft's actual control scheme will derive from this list and will be documented separately as it is developed.*

*Document still under review (and may always be)*

## Key Concepts

### Input Actions, Not Final Bindings

This document describes input actions and their default keyboard and mouse bindings. The actions are more important than the specific keys assigned to them. Individual games, control schemes, accessibility settings, and player preferences may bind these actions differently.

Not every action listed here is expected to be available in every game or interface mode.

### Hands Are Tools

Characters use their hands to interact with the world. A hand may be free, holding an item, using a tool, operating a control panel, carrying an object, climbing, fighting, or performing another action.

Some actions require one free hand. Some require both hands. Some can be performed while holding a tool or item, while others require the character to stow, drop, switch, or unequip whatever is in the way.

For example, a character generally cannot hold a tool and operate a control panel with the same hand at the same time. If the other hand is free, the player may switch to that hand or use it contextually.

### Characters Have Basic Athletic Competence

Most characters are assumed to be capable of ordinary physical movement beyond flat-ground walking. They should be able to step over small obstacles, climb onto low surfaces, lower themselves from ledges, crouch, crawl, jump, dive, swim, and recover from uneven terrain when physically able.

Some characters may also be capable of more advanced athletic movement depending on their abilities, equipment, encumbrance, injuries, exhaustion, environment, and training.

The controls express movement intent. The game decides how that intent is carried out based on context.

### Vertical Movement Is Core

Vertical movement is treated as a normal part of character control, not as a rare special case. Even games with primarily 2D movement may require climbing, swimming, dropping, stepping up, moving between levels, or navigating uneven terrain.

For that reason, upward and downward movement have dedicated input actions.

### Context Matters

Many actions change meaning depending on the character’s state, posture, equipment, vehicle, interface mode, and surroundings.

For example, the same upward movement input may mean stepping up, climbing, grabbing a surface, swimming upward, flying upward, or moving vertically in a grid. The same downward movement input may mean crouching, crawling, climbing down, swimming downward, or flying downward.

The goal is not to give every possible movement its own key. The goal is to give the player simple, consistent ways to express intent.

### Simple Actions Stay Immediate

Common actions should remain fast and direct. Advanced actions may use holds, double taps, radial menus, control panels, or contextual prompts, but ordinary movement and interaction should not require navigating menus during normal play.

### Control Panels Share One Interface

Character, inventory, suit, grid, NPC, map, and similar panels are treated as tabs or screens within a shared control-panel interface. Their separate input actions are shortcuts to open that shared interface directly to a specific panel.

For example, opening the inventory panel and opening the suit panel are not separate windows in principle. They are different entry points into the same overall control interface.

### Manual Control Is Not Always Primary

Some games may use these controls as the primary way the player drives the character, suit, vehicle, or grid. Other games may use point-and-click, command-based, or intent-based interaction as the primary interface, with manual controls available as an optional or advanced layer.

The same action vocabulary can support both styles.

## Movement

* **W** — `Move Forward` — press to move forward
* **A** — `Move Left` — press to move left
* **S** — `Move Backward` — press to move backward
* **D** — `Move Right` — press to move right
* **R** — `Move Up`
  * when walking or jogging:
    * will step up onto or down from small heights or over small obstacles while held
    * when crouching: will also climb down from ledges encountered while held
    * will grab large vertical surfaces while held
      * while holding surface, forward/left/right/backward movement is up/left/right/down on surface
      * will reverse to grab the surface if stepping off of it while moving forward
      * jump or boost jumps off the surface
    * can hold continuously to keep scaling or stepping up onto new vertical surfaces
  * when running:
    * will jump up onto or down from small heights or over small obstacles while held
  * when flying or swimming:
    * will move up while held
  * all scaling actions automatically toggle to standing
* **F** — `Move Down`
  * when standing, crouching, or crawling:
    * tapping will toggle to crouching if standing or crawling and to standing if crouching
    * holding will toggle to crawling if standing or crouching and to standing if crawling
  * when flying or swimming:
    * will move down while held
* **Space** — `JumpOrBoost`
  * when standing or crouching:
    * pressing starts a jump, hop, or boost
    * if released before full jump threshold, jump is executed with strength based on press duration (a hop)
    * if held beyond full jump threshold, jump is executed with full strength and boosts until released
    * if double tapped (starts as hop), acrobatic roll if standing and ground roll if crouching
    * if tap and hold (starts as hop), acrobatic dive (dive into crawling mode if near ground)
  * when flying, swimming, or in grid:
    * pressing boosts until released
* **Shift** (either side) — `RunModifier`
  * **Note:** shift is a modifier key, and so can only be assigned a modifier action
  * when on standing or crouching:
    * tap to toggle from walk or jog to running or from running to walk
    * double tap to jog continuously (tap to end)
      * jogging is the fastest non-tiring movement mode
      * it is faster than walking but slower than running
      * movement will toggle to walking when ended
    * hold to sprint forward while held
      * sprinting is the fastest movement mode, but also the most tiring
      * can only sprint forward
      * can not sprint while crouching
* **Ctrl** (either side) — `WalkModifier`
  * **Note:** ctrl is a modifier key, and so can only be assigned a modifier action
  * when standing or crouching:
    * without shift:tap to toggle from run or jog to walking or from walking to jog
    * with shift: tap to toggle careful walking mode
    * without shift: double tap to walk continuously (tap to end)
      * `MoveUp` has priority over moving carefully while held, but doesn't cancel it
    * with shift: double tap to walk continuously using performative style (tap to end)
    * tap and hold to for radial menu to select normal and performative walking styles
* **X** — `Brake`
  * tap to toggle braking mode (or clear reference)
  * hold to brake
  * tap and hold to set inertial reference
* **Z** — `FlightPack`
  * tap to `Toggle Flight Pack` (or boost pack)
  * double tap to `Toggle Boost Mode`
  * hold for radial menu to configure speed

## Rotation

* **Up Arrow** — `PitchUp`
* **Down Arrow** — `PitchDown`
* **Left Arrow** — `YawLeft`
* **Right Arrow** — `YawRight`
* **Q** — `RollLeft`
* **E** — `RollRight`
* **C** — `RotationalAlignment`
  * tap to toggle rotational alignment mode (or clear reference)
  * hold to align rotation
  * tap and hold to set rotational alignment reference
* **Alt** (either side) — `RotationalModifier`
  * **Note:** alt is a modifier key, and so can only be assigned a modifier action
  * in first person or chase view:
    * hold to rotate view (look around)
    * tap to recenter view (look straight ahead)
  * in chase view: tap and hold to select view position
  * in third person view:
    * hold to rotate camera around character
    * tap to recenter camera to chase position
  * **Note:** also modifies `ScrollUp` and `ScrollDown` to zoom in and out, respectively
* **V** — `ViewMode`
  * when in first person view: tap to toggle to chase view
  * when not in first person view: tap to toggle to first person view
  * when in third person view: double tap to toggle to chase view
  * when not in third person view: double tap to toggle to third person view
  * when in first person view: hold for third person view until released
  * when not in first person view: hold for first person view until released

## Interactions

> * just selecting an item through scrolling does not activate it, but makes `ToolbarAction` gestures and context specific actions available for that item
> * some items activate when selected using `ToolbarItem0`–`ToolbarItem9`, while others require `ToolbarAction` to activate

* **Mouse Wheel Down** or **]** — `ScrollDown`
  * without modifier: select next toolbar item (note: selects, but does not activate tools and items)
  * shift modified: select next toolbar
  * each hand stores its position in the toolbar, so switching hands will switch to the last selected item for that hand
  * alt modified: zoom out
* **Mouse Wheel Up** or **[** — `ScrollUp`
  * without modifier: select previous toolbar item (note: selects, but does not activate tools and items)
  * shift modified: select previous toolbar
  * each hand stores its position in the toolbar, so switching hands will switch to the last selected item for that hand
  * alt modified: zoom in
* **T** — `ToolbarAction`
  * toolbar items can be configured to perform a variety of actions with specific gestures and context
  * actions use the current hand, or both hands, unequipping any tools or items as needed
  * just selecting an item does not activate it, but makes `ToolbarAction` gestures and context specific actions available for that item
* **\`** — `SelectNone`
  * tap deselects all toolbar items for the current hand (acts as if hand is selected)
* **0**–**9** — `ToolbarItem0`–`ToolbarItem9`
  * without modifier: depending on item:
    * *either* activates the toolbar item using the current hand, or both hands, without selecting it
    * *or* selects the toolbar item for the current hand, without activating it
  * ctrl modified:
    * tap to select the toolbar item for the current hand, without activating it
    * hold to configure the toolbar item
  * alt modified: selects the toolbar for the current hand
  * Note: shift modifier used with some toolbar item gestures, so is not available for toolbar selection
* **Left Mouse Button** — `UsePrimary`
  * when tool is equipped (not just selected): primary tool action based on gestures and context specific to tool
  * when no tool is equipped:
    * in hand combat mode: hand combat primary action based on gestures and context
    * not in hand combat mode: manipulate object (move or rotate object)
* **Right Mouse Button** — `UseSecondary`
  * when tool is equipped (not just selected): secondary tool action based on gestures and context specific to tool
  * when no tool is equipped:
    * in hand combat mode: hand combat secondary action based on gestures and context
    * when not in hand combat mode: activate suit functions (if any) based on gestures and context
* **G** — `Tool`
  * double tap to switch active hand
  * when tool is selected: tap to equip or unequip tool
  * when no tool is selected: tap to enable/disable hand combat mode
  * when tool is equipped: hold to reload tool if it has a reload action (other hand must usually be free)
  * when no tool is equipped: hold to interact with target (triggers an action, may be continuous, or a radial menu)

## Control Panels and Devices

* **Esc** (fixed assignment) — `MainMenu`
  * without modifiers: tap to bring up main menu
    * will pause game in single player
  * shift modified: tap to bring up game console and log
  * hold to bring up radial menu of main menu actions (e.g., save, load, settings, etc.)
* **Tab** — `CharacterControlPanel`
  * tap to bring up control panel at character status screen
  * hold to bring up radial menu of character actions (e.g., emotes, gestures, etc.)
* **I** — `InventoryControlPanel`
  * tap to bring up control panel at inventory status screen (may be restricted to unmodified tap in future)
  * other gestures reserved for future inventory actions (e.g., planner actions)
* **J** — `SuitControlPanel`
  * tap to bring up control panel at suit status screen
  * hold to bring up radial menu of suit actions (e.g., toggle power, toggle lights, etc.)
  * suit control panel also controls flight and boost packs
* **K** — `GridControlPanel`
  * tap to bring up control panel at grid status screen
  * hold to bring up radial menu of grid actions (e.g., toggle power, toggle lights, etc.)
* **L** — `Lights`
  * tap to toggle selected lights on/off
  * entering a grid or suit will automatically select the lights selected for that grid or suit
  * exiting a grid while in a suit will automatically select the lights selected for that suit
  * hold to bring up radial menu of light actions (e.g., set light level, select/deselect light groups, etc.)
* **Y** — `Power`
  * tap to toggle selected power on/off
  * entering a grid or suit will automatically select the power selected for that grid or suit
  * exiting a grid while in a suit will automatically select the power selected for that suit
  * hold to bring up radial menu of power actions (e.g., set power level, select/deselect power groups, etc.)
* **P** — `Park`
  * tap to park or unpark grid or suit
  * tap and hold to auto park grid or suit (when possible)
  * hold for radial menu to select parking systems
* **O** — `RadioMode`
  * tap to toggle radio mode (broadcasting and receiving, receiving only, or off)
  * hold for radial menu to select broadcast systems
* **M** — `Map`
  * without modifier: tap to bring up map and navigation screen
  * without modifier: double tap to begin auto navigation to the selected destination
    * will preferentially use grid A.I. when in grid
    * will fallback to character A.I. when not in grid or when grid A.I. is not available
  * with ctrl modifier: tap create new GPS marker at current location
  * with ctrl modifier: hold to bring up new GPS marker dialogue
* **N** — `Npc`
  * tap to bring up control panel at NPC schedule screen
    * can schedule NPC behavior and actions
    * can assign missions to NPCs that override their normal schedule until cancelled
  * hold to bring up radial menu of NPC commands (e.g., follow, guard, etc.)
    * act as missions that override the NPC's normal schedule until cancelled
* **Enter** — `Chat`
  * tap to start or complete chat message
  * hold for radial menu to select chat channel
  * messages starting with `@` are sent to a specific player or group
    * can add trailing `:` for clarity (e.g., "@player1: message"), but is not required (e.g., "@player1 message")
    * use multiple for multiple players or groups (e.g., "@player1 @player2 @group1: message")
    * can comma separate multiple players or groups instead (e.g., "@player1,player2,group1: message")
  * messages starting with `/` execute commands (e.g., "/help", "/sleep", "/wave", or "/build")
    * some are A.I. commands (they activate the character's A.I. until completed or canceled)
      * A.I. commands can be queued
      * Can be directed at one or more NPC or grid A.I.s by preceding with "@name" list
      * Can target self with "me" and other characters and grids with their names
      * Examples:
        * "/build @grid1" — execute build plan on grid1
        * "@npc1,npc2: /build @grid1" — have npc1 and npc2 execute build plan on grid1
  * messages starting with `!` execute admin and debug commands (e.g., "!spawn", or "!teleport")

## Radial Menus

> * Deactivates other controls
> * Can also move mouse to select item in direction of movement

* **Q** or **Mouse Wheel Up** — `RadialMoveCounterClockwise`
* **E** or **Mouse Wheel Down** — `RadialMoveClockwise`
* **Space** or **Left Mouse Click** — `RadialSelect`
* **Backspace** or **Right Mouse Click** — `RadialCancel`
  * Moves back one selection or cancels radial menu if at top level
