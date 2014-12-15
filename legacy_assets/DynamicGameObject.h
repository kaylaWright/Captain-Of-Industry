//DynamicGameObjects handle all objects that are not stationary in the world. 
//Abstract; must have Update and Move overridden in all subclasses.
#pragma once

#include "GameObject.h"

class DynamicGameObject : public virtual GameObject
{
	
public:
	DynamicGameObject() {}
	~DynamicGameObject() {}

};