//InputController manages keyboard input as it relates to the player, in particular, 
// as well as the state of the world at large (GUI, other interactivities).
//control variables.
//Movement: WASD.
//Interact: Space -> will either check inventory or interact with object. 
//Q: Panhandle.
//E: Busk.
//I: Open inventory

#pragma once
#include <SDL_keycode.h>
#include "DynamicGameObject.h"

class InputController
{

public:
	InputController();
	~InputController();
	void KeyDown(SDL_Keysym _inKey);
	void KeyUp(SDL_Keysym _inKey);
	bool GetInventoryFlag();
	bool GetUp();
	bool GetDown();
	bool GetLeft();
	bool GetRight();
	bool GetInteract();
	bool GetBusk();
	bool GetPanhandle();
protected:
	//none.
private:
	bool inventoryKeyPressed;
	bool upPressed;
	bool leftPressed;
	bool downPressed;
	bool rightPressed;
	bool interactPressed;
	bool buskPressed;
	bool panHandlePressed;
};


