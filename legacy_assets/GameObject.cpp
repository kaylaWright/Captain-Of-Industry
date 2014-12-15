//Few methods common to all objects in game. 
//Most importantly is name; will be used to determine various interactions between the player and the world.

#pragma once 

#include "GameObject.h"

//Includes Get and Set functions common to all game objects.
//GetName, SetName.
#pragma region GetSet

std::string GameObject::GetName() const
{
	return name;
}

void GameObject::SetName(std::string _name)
{
	name = _name;
}
#pragma endregion GetSet
//K.W.
