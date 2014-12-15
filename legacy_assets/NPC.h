#ifndef NPC_H
#define NPC_H

#include "DynamicGameObject.h"

class NPC :
	public virtual DynamicGameObject
{
private:
	// AI mind set - affects what they think of homeless people in general
	enum AI_ATTITUDE {
		DISGUSTED,
		DISTRUSTFUL, 
		INDIFFERENT,
		HELPFUL,
		CARING,
		COMPASSIONATE
	};
	// AI wealth - dictates how much they will donate if they can donate
	enum AI_WEALTH {
		POOR,
		LOW_INCOME,
		AVERAGE,
		WELL_OFF,
		LOADED
	};
	// AI opinion - dictates whether the AI favors busking, panhandling, or is more likely
	// to donate when the play is doing neither of these (AKA - I donated cause I wanted to)
	enum AI_OPINION {
		BUSKING,
		PAN_HANDLING,
		IDLE
	};

private:
	AI_ATTITUDE mindset;
	AI_WEALTH wealth;
	AI_OPINION opinion;
	int cashOnPerson;

public:
	NPC(std::string _name, SDL_Texture *_pic, int _x, int _y);
	virtual ~NPC();

	void update();
	void move(DIRECTION _direction);
	// check donation function should take in player skills, compare with AI vals, and return an amount to give
	// int checkDonation
};

#endif

