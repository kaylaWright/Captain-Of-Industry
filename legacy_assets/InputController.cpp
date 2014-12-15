//InputController manages keyboard input as it relates to the player, in particular, 
// as well as the state of the world at large (GUI, other interactivities).
#include "InputController.h"
#include <iostream>
//Constructor/Destructor contains just that.
#pragma region Constructor/Destructor
InputController::InputController(void) : 
	upPressed(false), 
	leftPressed(false), 
	downPressed(false), 
	rightPressed(false), 
	interactPressed(false), 
	buskPressed(false), 
	panHandlePressed(false),
	inventoryKeyPressed(false)
{}

InputController::~InputController(void)
{}

#pragma endregion Constructor/Destructor

//Contains the functions responsible for delegating out information on a per-key basis.
//KeyDown, KeyUp
#pragma region Events

// Receives SDL key input from main loop.
// _key in both functions refers to the SDL_code for a given keypress.
void InputController::KeyDown(SDL_Keysym _inKey)
{
	switch(_inKey.sym)
	{
	case SDLK_w:
		upPressed = true;
		break;
	case SDLK_a:
		leftPressed = true;
		break;
	case SDLK_s:
		downPressed = true;
		break;
	case SDLK_d:
		rightPressed = true;
		break;
	case SDLK_q:
		buskPressed = true;
		break;
	case SDLK_e:
		panHandlePressed = true;
		break;
	case SDLK_SPACE:
		interactPressed = true;
		break;
	case SDLK_i:
		inventoryKeyPressed = true;
	default:
		break;
	}
}
void InputController::KeyUp(SDL_Keysym _inKey)
{
	switch(_inKey.sym)
	{
	case SDLK_w:
		upPressed = false;
		break;
	case SDLK_a:
		leftPressed = false;
		break;
	case SDLK_s:
		downPressed = false;
		break;
	case SDLK_d:
		rightPressed = false;
		break;
	case SDLK_q:
		buskPressed = false;
		break;
	case SDLK_e:
		panHandlePressed = false;
		break;
	case SDLK_SPACE:
		interactPressed = false;
		break;
		default:
			break;
	}
}

#pragma endregion Events

//Contains all of the Gets and Sets (in theory) that will be used to make decisions in the Game Manager class.
//Get GetUp(),  GetDown(), GetLeft(), GetRight(), GetInteract(), GetBusk(), GetPanhandle(), 
#pragma region Get/Set

bool InputController::GetUp()
{
	return upPressed;
}

bool InputController::GetDown()
{
	return downPressed;
}

bool InputController::GetLeft()
{
	return leftPressed;
}

bool InputController::GetRight()
{
	return rightPressed;
}

bool InputController::GetInteract()
{
	return interactPressed;
}

bool InputController::GetBusk()
{
	return buskPressed;
}

bool InputController::GetPanhandle()
{
	return panHandlePressed;
}

bool InputController::GetInventoryFlag()
{
	return inventoryKeyPressed;
}

#pragma endregion Get/Set

