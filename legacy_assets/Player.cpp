#pragma once
#include <iostream>
#include "Player.h"

//Constructor contains only the Constructor and, if necessary at a later point, the destructor.
#pragma region Constructor

Player::Player(std::string _name, SDL_Texture &_pic, InputController* _input, int _x, int _y) : 
	HUNGERSTEP(0), 
	THIRSTSTEP(0), 
	SANITYSTEP(0), 
	hungerSated(0), 
	thirstSlaked(0), 
	sanityRecuped(0), 
	buskingSkill(0), 
	panhandlingSkill(0), 
	survivalSkill(0), 
	timesSearched(0), 
	totalTimeSpentPanhandling(0), 
	totalTimeSpentBusking(0), 
	totalTimesSearched(0), 
	goalTimeSpentPanhandling(100), 
	goalTimeSpentBusking(100), 
	encumbrance(0), 
	maxEncumbrance(10)
{
	SetDirection(DOWN);
	//*** TWEAK ***
	hunger = static_cast<int>(floor(rand() % 25 + 10));
	thirst = static_cast<int>(floor(rand() % 25 + 10));
	sanity = static_cast<int>(floor(rand() % 25 + 10));

	this->SetName(_name);
	x = _x;
	y = _y;
	isCollidable = false;
	image = &_pic;
	input = _input;
}

#pragma endregion Constructor

//Updates contains any functions required to update dear hobo's status. 
//Update, UpdateHunger, UpdateThirst, UpdateSanity, UpdateCondtions, and CheckLife
#pragma region Updates
void Player::update()
{
	//updating player status.
	UpdateHunger(hungerSated);
	UpdateThirst(thirstSlaked);
	UpdateSanity(sanityRecuped);

	//reset the player status variables.
	hungerSated = 0;
	thirstSlaked = 0;
	sanityRecuped = 0;

	//**???**//
	if(isPanhandling)
	{
		timeSpentPanhandling++;
	}

	if(isBusking)
	{
		timeSpentBusking++;
	}
	this->move();
}

void Player::UpdateHunger(int _hungerSated)
{
	hunger -= HUNGERSTEP;
	hunger += _hungerSated;

	// hunger = [0, 100]
	if(hunger > 100)
	{
		hunger = 100;
	}

	if(hunger < 0)
	{
		hunger = 0;
	}
}

void Player::UpdateThirst(int _thirstSlaked)
{
	thirst -= THIRSTSTEP;
	thirst += _thirstSlaked;

	// thirst = [0, 100]
	if(thirst > 100)
	{
		thirst = 100;
	}

	if(thirst < 0)
	{
		thirst = 0;
	}
}

void Player::UpdateSanity(int _sanityRecuped)
{
	sanity -= SANITYSTEP;
	sanity += _sanityRecuped;

	//sanity = [0, 100]
	if(sanity > 100)
	{
		sanity = 100;
	}

	if(sanity < 0)
	{
		sanity = 0;
	}
}

void Player::UpdateConditions()
{
	//check to see if we have any conditions on our character. 
	//if we do, then we need to check to see how long remains on them, and remove/apply them as needed. 
}

bool Player::CheckLife()
{
	//if any of our stats are below zero, the hobo has left this earth.
	if(hunger <= 0 || thirst <= 0 || sanity <= 0)
	{
		return false;
	}

	return true;
}

#pragma endregion Updates

//Player Actions includes all actions direct and indirect, taken by the player. 
//This also contains the functions that respond to change in the input controller. 
//Move, ChangeState, Busk, Panhandle, Interact, Search, and CheckInventory.
#pragma region Player Actions

//advanced move function, receives input from input controller and delegates out to a more basic move function 
//as well as delegating to other player actions.
void Player::move()
{
	if (input->GetDown() && !input->GetUp())
	{
		move(DOWN);
		SetDirection(DOWN);
	}

	if (input->GetUp() && !input->GetDown())
	{
		move(UP);
		SetDirection(UP);
	}

	if (input->GetLeft() && !input->GetRight())
	{
		move(LEFT);
		SetDirection(LEFT);
	}

	if (input->GetRight() && !input->GetLeft())
	{
		move(RIGHT);
		SetDirection(RIGHT);
	}

	if (input->GetInteract())
	{
		Interact();
	}

	if (input->GetBusk())
	{
		Busk();
	}

	if (input->GetPanhandle())
	{
		Panhandle();
	}
}

//basic move function, actually handles character movement.
void Player::move(DIRECTION _direction)
{
	switch (_direction)
	{
		case UP:
			if (y > 0 && !m_MapCollisionData[x][y-1])
				y -= 1;
			break;
		case LEFT:
			if (x > 0 && !m_MapCollisionData[x-1][y])
				x -= 1;
			break;
		case DOWN:
			if (y < 32 && !m_MapCollisionData[x][y+1])
				y += 1;
			break;
		case RIGHT:
			if (x < 32 && !m_MapCollisionData[x+1][y])
				x += 1;
			break;
		default:
			//nothing.
			break;			
	}
}

void Player::Busk()
{
	if(!isBusking)
	{
		isBusking = true;
		timeSpentBusking = 0;
	}
	else
	{
		isBusking = false;
		totalTimeSpentBusking += timeSpentBusking;

		if(totalTimeSpentBusking >= goalTimeSpentBusking)
		{
			buskingSkill += static_cast<int>(floor(rand() % 4 + 1));
			goalTimeSpentBusking *= 2;
		}
	}
}

void Player::Panhandle()
{
	if(!isPanhandling)
	{
		isPanhandling = true;
		timeSpentPanhandling = 0;
	}
	else
	{
		isPanhandling = false;
		totalTimeSpentPanhandling += timeSpentPanhandling;

		//*** TWEAK **
		if(totalTimeSpentPanhandling >= goalTimeSpentPanhandling)
		{
			panhandlingSkill += static_cast<int>(floor(rand() % 4 + 1));
			goalTimeSpentPanhandling *= 2;
		}
	}
}

void Player::Interact()
{
	//if touching an interactive object, interact with it/seaarch it..
	//otherwise, check your inventory? 
}

void Player::Search()
{
	//increase the times you've searched; may increase the survival stat.
	timesSearched++;

	//potentially increase the skill.
	//*** TWEAK ***
	if(timesSearched >= totalTimesSearched)
	{
		totalTimesSearched += timesSearched;
		timesSearched = 0;

		survivalSkill += static_cast<int>(floor(rand() % 4 + 1));
	}

	//check an interactive object to see what it contains.
	//**SEE SEAN
}

void Player::CheckInventory()
{
	//check your inventory to see what it contains.
}

#pragma endregion Player Actions

//K.W.

// Reinit gameplay values to start a new game
void Player::Reset()
{
	SetDirection(DOWN);
	//*** TWEAK ***
	hunger = static_cast<int>(floor(rand() % 25 + 10));
	thirst = static_cast<int>(floor(rand() % 25 + 10));
	sanity = static_cast<int>(floor(rand() % 25 + 10));

	x = 3;
	y = 4;
	isCollidable = false;
}

