#include <iostream>
#include <SDL_image.h>
#include <vector>

#include "SDL.h"
#include "Timer.h"
#include "InputController.h"
#include "TMXLoader.h"
#include "ObjectFactory.h"

// basic states for main loop logic
enum GAMESTATE
{
	MENU, 
	GAME_PLAY,
	INVENTORY, 
	SHOP,
	GAME_OVER
};
GAMESTATE gState = MENU;

// This value determines how often to call animation updates
#define FRAMESPERSECOND 30
// Width and Height of game window
const int SCREEN_HEIGHT = 1024;
const int SCREEN_WIDTH  = 1024;
// Create a window for sdl to use
SDL_Window *window = nullptr;
// Create a renderer to draw to the window
SDL_Renderer *renderer = nullptr;
// Create a texture to store the map
SDL_Texture *mapTexture = nullptr;
// Call function at program end to free all memory allocated with SDL classes
void FreeSDLMemory();
//init functions
bool init();
// Clears render, and draws all objects in a given factory
void drawScene(ObjectFactory* objectFactory);
void loadMap();

SDL_Surface *surface = nullptr;
TMXLoader* tmxLoader = NULL;

int main( int argc, char* args[] )
{
	bool quit = false;
	//Initialize
    if( init() == false )
    {
        return 1;
    }
	
	// Create SDL event for key listener
	SDL_Event sdl_event;
	// Create input controller to pass to Player
	InputController* pInput = new InputController();
	// Load splash screen and game over sprites
	SDL_Texture* menu = IMG_LoadTexture(renderer, "sprites/menu.png");
	SDL_Texture* gameOver = IMG_LoadTexture(renderer, "sprites/gameOver.png");
	// Load player sprite
	SDL_Texture* text = IMG_LoadTexture(renderer, "sprites/Player.png");
	// Create object factory and push player into it
	Player* player = new Player("hobo", *text, pInput, 3, 4);
	player->SetMapCollisionData(tmxLoader->getMapCollisionData());
	ObjectFactory objectFactory(renderer);
	objectFactory.AddObject(player);
	// Setup a new level to play
	objectFactory.SetupLevel(renderer);
	// Create and start timer
	Timer timer;
	timer.Start();
	/* Main game loop
	** Poll SDL events, pass into player controller
	** Increment game timer, perform game logic
	** Perform AI logic
	*/
	while (!quit)
	{
        // SDL Event polling for keyboard input
		// Passes key presses to player input controller as necessary
        while (SDL_PollEvent(&sdl_event))
		{
            // If user closes the window
            if (sdl_event.type == SDL_QUIT)
                quit = true;
			 // If user presses the escape key
			if (sdl_event.type == SDL_KEYDOWN && sdl_event.key.keysym.scancode == SDL_SCANCODE_ESCAPE)
                quit = true;

            // If user presses/release any key other than escape, pass the key to input controller
			if (sdl_event.type == SDL_KEYDOWN)
			{
				
				pInput->KeyDown(sdl_event.key.keysym);
			}
			else if (sdl_event.type == SDL_KEYUP)
			{
				// if enter is pressed at menu, check state and move
				if (sdl_event.key.keysym.scancode == SDL_SCANCODE_RETURN)
				{
					switch (gState)
					{
					case MENU:
						player->Reset();
						gState = GAME_PLAY;
						break;
					case GAME_OVER:
						gState = MENU;
						break;
					}
				}
				// otherwise pass input to controller
				else
					pInput->KeyUp(sdl_event.key.keysym);
			}
			if (pInput->GetInventoryFlag() == true)
			{
				gState = INVENTORY;
				break;
			}
		}
		// basic state machine for game logic
		switch (gState)
		{
		// main menu case, just draw a menu and wait for input
		case MENU:
			SDL_RenderCopy(renderer, menu, NULL, NULL);
			SDL_RenderPresent(renderer);
			break;
		case GAME_PLAY:
			// Increment our game timer
			if (timer.IncrementTime() == true)
			{
				if (player->CheckLife() == false)
				{
					gState = GAME_OVER;
				}
				objectFactory.Update();
			}
			//std::cout << timer.GetDeltaTime() << " xx " << timer.GetDeltaTimeMS() << " \n";
			drawScene(&objectFactory);
			break;
		case INVENTORY:
			break;
		case SHOP:
			break;
		case GAME_OVER:
			SDL_RenderCopy(renderer, gameOver, NULL, NULL);
			SDL_RenderPresent(renderer);
			break;
		}
	}
	FreeSDLMemory();
	SDL_Quit();
	return 0;
}

bool init ()
{
	// ** SDL Setup Code Block
	// Initialise SDL, if an error is thrown (-1)
	// Print error to console with cout
	if (SDL_Init(SDL_INIT_EVERYTHING) == -1){
		std::cout << SDL_GetError() << "\n";
        return false;
    }
	// SDL_CreateWindow (Window Name, x pos, y pos, height, width, SDL_FLAGS)
    window = SDL_CreateWindow("Hobo: Captain of Industry", 100, 100, SCREEN_HEIGHT, SCREEN_WIDTH, SDL_WINDOW_SHOWN);
    // If win is still a nullptr after creation, printout error to console
	if (window == nullptr){
		std::cout << "SDL_CreateWindow Error: " << SDL_GetError() << "\n";
        return false;
    }
	// Create Renderer
	// check parameters for this function, unsure of meaning
    renderer = SDL_CreateRenderer(window, -1, SDL_RENDERER_ACCELERATED | SDL_RENDERER_PRESENTVSYNC);
    if (renderer == nullptr) {
        std::cout << "SDL_CreateRenderer Error: " << SDL_GetError() << "\n";
        return false;
    }
	SDL_SetRenderDrawColor(renderer, 255, 255, 255, 1);
	SDL_RenderClear(renderer);

// ** SDL Setup Block ends here

	tmxLoader = new TMXLoader();
	// this function should be parameterized, fix this in future
	tmxLoader->loadDocument();
	loadMap();
	
	// all initialization successful
	return true;


}

void loadMap ()
{
	SDL_Surface* tileset = tmxLoader->getMapImage();
	if (tileset != NULL)
	{
        int x = 0;
        int y = 0;

		mapTexture =  SDL_CreateTextureFromSurface(renderer, tileset);
		SDL_FreeSurface(tileset);		
    }
}

void drawScene (ObjectFactory* objectFactory)
{
	// the two NULL parameters represent the sourceRectangle and destinationRectangle. source should be the rectangle representing the portion of the map we 
	// want to draw, destination should be the area of the window we want to draw to
	SDL_RenderCopy(renderer, mapTexture, NULL, NULL);
	// after clearing screen to map texture, draw objects in factory
	objectFactory->DrawAll(renderer);
	SDL_RenderPresent(renderer);
}

// Clear all pointers created with SDL classes
void FreeSDLMemory()
{
	SDL_DestroyRenderer(renderer);
    SDL_DestroyWindow(window);
	SDL_FreeSurface(surface);
}
