#ifndef OBJECT FACTORY_H
#define OBJECT_FACTORY_H

#include <vector>
#include <SDL_image.h>
#include "GameObject.h"
#include "Player.h"
#include "NPC.h"


// aka WORLD ENGINE
class ObjectFactory
{
private:
	// Structure of all objects loaded into factory
	std::vector<GameObject*> objects;
	// Values for tracking total objects in array, and max objects allowed
	int totalObjects;
	// Load NPC sprites
	SDL_Texture* npcs[3];

public:
	ObjectFactory(SDL_Renderer* _renderer, int _startSize = 25);
	// Get total size of object array
	int TotalObjectCount() { return totalObjects; }
	// Get object functions
	GameObject* GetObject(int _index);
	GameObject* GetObject(std::string _name);
	GameObject* GetObject(int _x, int _y);
	// Add an object to the array
	void AddObject(GameObject* _newObject);
	// Draw all visuals for objects in factory
	void DrawAll(SDL_Renderer* renderer);
	// Logic performed on objects as needed
	void Update();
	// Destroy object functions
	bool Destroy(GameObject *target);
	bool Destroy(std::string _name);
	bool Destroy(int _x, int _y);
	// Load game objects in for a level (to be randomized)
	void SetupLevel(SDL_Renderer* renderer);
	// Erases all objects in array
	void Clear();
	virtual ~ObjectFactory(void) {}

private:
	// Private destroy function, called within public destroy functions.
	// This swaps the index and last position object and then pops last
	bool DestroyAtIndex(int _index);
};

#endif
// MD