#include "Timer.h"

Timer::Timer(bool _startNow) :
	lastFrameTime(0), 
	startTime(0), 
	gameDelay(FRAMEDELAY)
{
	isRunning = _startNow;
}

void Timer::Start()
{
	startTime = SDL_GetTicks();
	lastFrameTime = startTime;
	isRunning = true;
	gameDelay = FRAMEDELAY;
}

bool Timer::IncrementTime()
{ 
	if (isRunning == true)
	{
		gameDelay -= 1;
		lastFrameTime = SDL_GetTicks();
		if (gameDelay <= 0)
		{
			gameDelay = FRAMEDELAY;
			return true;
		}
	}
	return false;
}

float Timer::GetElapsedTime()
{
	return SDL_GetTicks() - startTime;
}

void Timer::Stop()
{
	isRunning = false;
}

Timer::~Timer() {}