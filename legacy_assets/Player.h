#pragma once

#include <math.h>
#include "DynamicGameObject.h"
#include "InputController.h"

// DEBUG used for temp collision handling
#include <vector>

class Player : public DynamicGameObject
{
private:
	InputController *input;

	const int HUNGERSTEP;
	const int THIRSTSTEP;
	const int SANITYSTEP;

	int hunger;
	int thirst;
	int sanity;

	int hungerSated;
	int thirstSlaked;
	int sanityRecuped;

	int buskingSkill;
	int panhandlingSkill;
	int survivalSkill; 

	bool isBusking;
	bool isPanhandling;

	int goalTimeSpentPanhandling;
	int goalTimeSpentBusking;
	int timeSpentPanhandling;
	int timeSpentBusking;
	int timesSearched;
	int totalTimeSpentPanhandling;
	int totalTimeSpentBusking;
	int totalTimesSearched;

	int encumbrance;
	int maxEncumbrance;

	void UpdateHunger(int _hungerSated = 0);
	void UpdateThirst(int _thirstSlaked = 0);
	void UpdateSanity(int _sanityRecuped = 0);
	void UpdateSanity();
	void UpdateConditions();

public:

	Player(std::string _name, SDL_Texture &_pic, InputController* _input, int _x, int _y);
	~Player();
	bool CheckLife();
	void update();
	void move();
	void move(DIRECTION _direction);
	void Busk();
	void Panhandle();
	void Interact();
	void Search();
	void CheckInventory();
	// Reinit values for a new game
	void Reset();

	// DEBUG used for temporary collision handling
	std::vector< std::vector< bool > > m_MapCollisionData;
	void SetMapCollisionData (std::vector< std::vector< bool > > mapCollisionData) { m_MapCollisionData = mapCollisionData; }
};

//K.W.