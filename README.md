# game_works
 ## Event driven framework for making games in Unity
 
 **!Warning!** Work in progress, much more documentation needs to be written, but the framework is already usable.<br>
 
## What it is

A framework as a base for your unity game. This will let you make a clean and modular game without creating spaghetti code.<br>
The framework uses the unity event system, and lets your game objects and code communicate efficiently with events.<br>
Add the event system classes to an object in your first scene of the game, with the SystemObject script, and they will run and stay alive throughout scene/level changes.<br>
They manage **communication, scene changes, constants, states of the game, controls and storing progress or anything in cache.**<br>


## What the future holds

I want to add more and more basic functions, that are often used in games, as I develop them in the game that i am currently working on. Like movement, camera control, basic ai-npc behaviour, movement-trajectories...

## Setup

Add the event system classes to an object in your first scene of the game, with the SystemObject script, and they will run and stay alive throughout scene/level changes.<br>
For an example on how to listen and send events, have a look at the ObjectScripts/InGame/TriggerZone.cs file.<br>
Have a look at the EventSystem/EventSafe.cs file on how you can safe and access events.<br>
If you want to add your own events with their own data, check out the EventSystem/GWEventManager.cs and EventSystem/GWConstManager.cs files and the EventSystem/DataClasses/EventValues folder.<br>
Constants are stored in EventSystem/GWConstManager.cs, and other global variables or the global state, in EventSystem/GWStateManager.cs<br>
You can find many usefull files for your game in the ObjectScripts folder.<br>