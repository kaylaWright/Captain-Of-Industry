#include "ObjectFactory.h"
#define TILESIZE 32
#define SPRITERESOLUTION 32

ObjectFactory::ObjectFactory(SDL_Renderer* _renderer, int _startSize) : totalObjects (0)
{
	// Create object array with new max and allocate memory
	objects.reserve(_startSize);
	// Load NPC sprite assets
	npcs[0] = IMG_LoadTexture(_renderer, "sprites/MattNPC.png");
	npcs[1] = IMG_LoadTexture(_renderer, "sprites/SeanNPC.png");
	npcs[2] = IMG_LoadTexture(_renderer, "sprites/AndrewNPC.png");
}
void ObjectFactory::AddObject(GameObject* _newObject)
{
	// If total objects has reached negative value, reset to 0 (debug line as this should not happen)
	if (totalObjects == -1)
		totalObjects = 0;
	// Add object to highest position, then increment total objects
	if (_newObject)
	{
		objects.push_back(_newObject);
		totalObjects++;
	}
}
void ObjectFactory::Clear()
{
	objects.clear();
	totalObjects = 0;
}
void ObjectFactory::Update()
{
	for (int i = 0; i < totalObjects; i++)
	{
		objects[i]->update();
	}
}
void ObjectFactory::DrawAll(SDL_Renderer* renderer)
{	
	// Create rectangle for drawing to screen
	SDL_Rect* screenRect = new SDL_Rect();
	screenRect->h = TILESIZE;
	screenRect->w = TILESIZE;
	// Create rectangle for splitting image into frames
	SDL_Rect* textureRect = new SDL_Rect();
	textureRect->h = SPRITERESOLUTION;
	textureRect->w = SPRITERESOLUTION;
	// Iterate through objects and draw
	for (int i = 0; i < totalObjects; i++)
	{
		// Set splitting texture to height based on frame enum in game Object
		// This offsets y position by a value of (0-3 * sprite resolution)
		textureRect->y = objects[i]->GetDirection() * SPRITERESOLUTION;
		// Set x and y coordinates of screen position based on objects x and y
		// multiplied by tileSize of map
		screenRect->x = objects[i]->x * TILESIZE;
		screenRect->y = objects[i]->y * TILESIZE;
		// Finall pass paramenters to SDL to draw appropriate frame and position
		SDL_RenderCopy(renderer, objects[i]->image, textureRect, screenRect);
	}
	delete screenRect;
	delete textureRect;
}
#pragma region DestroyFunctions
bool ObjectFactory::Destroy(GameObject *target)
{
	// Iterate through and find matching object
	for (int i = 0; i < totalObjects; i++)
	{
		// When match is found call Destroy with index
		if (objects[i] == target)
			return DestroyAtIndex(i);
	}
	// Returns false if target was not found
	return false;
}
bool ObjectFactory::Destroy(std::string _name)
{
	// Iterate through and find matching object
	for (int i = 0; i < totalObjects; i++)
	{
		// When match is found call Destroy with index
		if (_name == objects[i]->GetName())
			return DestroyAtIndex(i);
	}
	// Returns false if target was not found
	return false;
}
bool ObjectFactory::Destroy(int _targetX, int _targetY)
{
	// Iterate through and find matching object
	for (int i = 0; i < totalObjects; i++)
	{
		// When match is found call Destroy with index
		if (objects[i]->x == _targetX)
			if (objects[i]->y == _targetY)
			return DestroyAtIndex(i);
	}
	// Returns false if target was not found
	return false;
}
bool ObjectFactory::DestroyAtIndex(int _index)
{
	// Ensure index requested is within bounds of exisiting objects
	if (_index > totalObjects || totalObjects <= 0)
		return false;

	// Swap given index with last object in array, and decrement size
	totalObjects--;
	objects[_index] = objects[totalObjects];
	objects.pop_back();
	return true;
}
#pragma endregion DestroyFunctions
#pragma region GetObjectFunctions
GameObject* ObjectFactory::GetObject(int _index)
{
	if (_index > -1 && _index < totalObjects)
		return objects[_index];
}
GameObject* ObjectFactory::GetObject(std::string _name)
{
	for (int i = 0; i < totalObjects; i++)
	{
		if (objects[i]->GetName() == _name)
			return objects[i];
	}
}
GameObject* ObjectFactory::GetObject(int _x, int _y)
{
	for (int i = 0; i < totalObjects; i++)
	{
		if (objects[i]->x == _x && objects[i]->y == _y)
			return objects[i];
	}
}
#pragma endregion GetObjectFunctions

void ObjectFactory::SetupLevel(SDL_Renderer* renderer)
{
	this->AddObject(new NPC("jerk", npcs[0], 11, 10));
	this->AddObject(new NPC("jerk", npcs[1], 7, 22));
	this->AddObject(new NPC("jerk", npcs[2], 24, 16));

	this->AddObject(new NPC("jerk", npcs[0], 23, 7));
	this->AddObject(new NPC("jerk", npcs[0], 24, 7));
	this->AddObject(new NPC("jerk", npcs[0], 25, 7));
	this->AddObject(new NPC("jerk", npcs[0], 23, 8));
	this->AddObject(new NPC("jerk", npcs[0], 24, 8));
	this->AddObject(new NPC("jerk", npcs[0], 25, 8));
}

// MD