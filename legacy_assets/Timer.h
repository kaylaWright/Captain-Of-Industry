#ifndef TIMER_H
#define TIMER_H
#include "SDL.h"
#define FRAMEDELAY 5
class Timer
{
private:
	// Is the timer running
	bool isRunning;
	// Store the time in milliseconds at the last frame
	Uint32 lastFrameTime;
	// Store when the timer was started
	Uint32 startTime;
	// Used to delay updates to game world
	Uint32 gameDelay;

public:
	Timer(bool _startNow = false);
	virtual ~Timer();
	// Start the timer
	void Start();
	// Stop the timer
	void Stop();
	// Increment clock time - call every update
	bool IncrementTime();
	// Return total time (in ms)
	float GetElapsedTime();
	// Find the time elapsed in seconds
	float GetDeltaTimeMS() { return (SDL_GetTicks() - lastFrameTime)*0.001; }
	// Find the time elapsed between frames in milliseconds
	Uint32 GetDeltaTime() { return (SDL_GetTicks() - lastFrameTime); }
};
#endif