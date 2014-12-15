#pragma once 

#include <string>
#include "SDL.h"

class GameObject
{
private:
	std::string name;

protected:
	
	enum DIRECTION {
		DOWN, 
		LEFT, 
		RIGHT, 
		UP
	};
	DIRECTION direction;

public:
	SDL_Texture *image;
	int x;
	int y;
	bool isCollidable;
	void SetName(std::string _name);
	std::string GetName() const;

	DIRECTION GetDirection() { return direction; }
	void SetDirection(DIRECTION _d) { direction = _d; }

	virtual void move(DIRECTION _d) = 0;
	virtual void update() = 0;
};

//K.W.