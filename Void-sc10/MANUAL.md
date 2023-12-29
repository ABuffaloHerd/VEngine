# The Void Combat Tech Demo

Welcome! Let me be the first to congratulate you on downloading this bare boned migraine of a video game.
Since I have no idea how to implement tutorials of any sort, read this manual to figure out what's happening.

In the ARENA, characters and objects are represented by abstract letters and symbols. Just know that whatever happens, your currently controlled character flashes at the start of their turn.

Use the WASD keys to move around. Turn your character with the arrow keys.
At the bottom left of the HUD panel, there is a value labelled 'M'. Characters can only move this many tiles per turn.

Q: "What direction is my character facing?"
A: I don't know. Just press an arrow key to make sure they're looking the right way.

Press J to highlight the current character's weapon range. Objects in this range are what the weapon hits. Closest object gets priority. You can use this to check what direction your character is facing.
Press the spacebar to relinquish the current character's turn and pass it to the next one in the queue. You can see who's up next by looking at the TURN panel.

There are two classes implemented so far: Mage and Ranger.
Mages can summon magic circles. Click the "summon" button in the controls panel to summon one. The circle will occupy the first available space in a clockwise + shape around the character.
Each magic circle grants the owner +10 max MP and +5 MP regen. Stats are added and recalculated at the start of every turn. Right now the magic circles can't tell who's the owner so everyone benefits.

Ranger uses ammo to attack. They can't reload right now so it's really funny to see them shoot nothing.