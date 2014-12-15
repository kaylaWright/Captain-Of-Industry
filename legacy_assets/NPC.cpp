#include "NPC.h"

NPC::NPC(std::string _name, SDL_Texture *_pic, int _x, int _y)
{
	x = _x;
	y = _y;
	image = _pic;
	SetDirection(DOWN);
}

void NPC::move(DIRECTION _direction)
{
}

void NPC::update()
{
}
NPC::~NPC(void)
{

}
