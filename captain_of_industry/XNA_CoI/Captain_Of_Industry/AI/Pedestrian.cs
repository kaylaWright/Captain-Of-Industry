using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Captain_Of_Industry
{
    public enum MoveDirection { NORTH, EAST, SOUTH, WEST, NONE } ;
    
    //Pedestrians will walk through the map on their way to some unknown and (to our hobo) unimportant destination.
    //They have a chance of giving money to the player character, depending on -> what the player is doing, their generosity, and their relative affluence.
    //they do not donate if they've donated previously to anyone. 
    //they will have a finite, randomly generated amount of money to give -> typically between 0.25$ and 2$, but occasional 'exceptions' will allow donations of 5$, 10$, or 20$.
    public class Pedestrian : DynamicObject
    {
        //consts
        private const double MAX_DONATIONCHANCE = 0.35d;
        private const float MAX_DONATIONDISTANCE = 40.0f;
        private const float DONATIONOFFSET = 0.30f;

        //movement related. 
        MoveDirection moveDir;
        float movementSpeed;
        Vector2 startPos;

        // donation-related stats/
        protected double donationChance;        //determines the odds the pedestrian will donate as they pass the player; generated in constructor from randomized generosity/affluence.
        protected bool hasDonated = false;      //if the pedestrian has donated to any hobo in passing, they will NOT donate again. 
        protected bool willDonate = true;       //toggles donation possibilities over time.

        //location in the game world, used to leave/enter buildings.
        protected TILE_TYPE tileToNorth;

        //internally useful variables, used for calculations.
        Random rnd;

        public Pedestrian(Animation _animation, Vector2 _position) : base(OBJECT_TYPE.NPC)
        {
            rnd = new Random();

            this.SetName("Pedestrian");
            this.isCollidable = true;

            //determine a semi-randomized starting-position based off of sidewalks. 
                //**position = -->access to roads here, choose one, position at one end or the other.
            //determine currentDir based off of map and position -> need map implementation before we do this.
                //currentDir = -->difference between position and map space, effectively.
            startPos = _position;

            if (rnd.NextDouble() > 0.5)
            {
                moveDir = MovementDirection.EAST;
            }
            else
            {
                moveDir = MovementDirection.WEST;
            }
            //probably calls the initialize function here? 

            //random generosity between 0-1.
            double generosity = rnd.NextDouble();
            //random affluence between 0-1.
            double affluence = rnd.NextDouble();
            //randomized donationChance -> 15-35% donation chance from the get go
            donationChance = Math.Min((generosity + affluence), MAX_DONATIONCHANCE);

            base.Initialize(_animation, _position);
        }

        public override void Update(GameTime _gameTime)
        {
            //move the sprite.
            Move();

            base.Update(_gameTime);
        }

        public void Move()
        {
            if (startPos.X - 100 > position.X)
            {
                moveDir = MoveDirection.EAST;
            }

            if (startPos.X + 100 < position.X)
            {
                moveDir = MoveDirection.WEST;
            }

            if (startPos.Y - 100 > position.Y)
            {
                moveDir = MoveDirection.SOUTH;
            }

            if (startPos.Y + 100 < position.Y)
            {
                moveDir = MoveDirection.NORTH;
            }

            switch (moveDir)
            {
                case MoveDirection.NORTH:
                    position.Y -= 1;
                    break;
                case MoveDirection.EAST:
                    position.X += 1;
                    break;
                case MoveDirection.SOUTH:
                    position.Y += 1;
                    break;
                case MoveDirection.WEST:
                    position.X -= 1;
                    break;
            }

        }

        private bool DetermineDonation(Player _target)
        {
            //check and see if the chance of donating exceeds the minimum value of wanting to donate. 
            if (!hasDonated && donationChance >= (rnd.NextDouble() + DONATIONOFFSET))
            {
                //donate to closest hobo
                _target.GrowRicher(DetermineDonatedMoney());
                hasDonated = true;
                willDonate = false;
                return true;
            }

            //else, don't donate at all -> failed donation check. 
            return false;
        }
        
        private float DetermineDonatedMoney()
        {
            //randomize small chances of donating larger volumes of money.
            int cash = (int)(rnd.NextDouble() * 100);

            if (cash <= 1)
                //20.00$
                return 20.0f;
            if (cash <= 3)
                //10.00$
                return 10.0f;
            if (cash <= 7)
                //5.00$
                return 5.0f;
            else
                //donate somewhere between 0.25$ and 2.00$.
                return Math.Max((float)(rnd.Next(0, 2) + 0.25), 2.0f);
        }

        //collision handling.
        //ai should move out of the way of other people, by randomly choosing a direction 
        //different from the direction they're in, moving that way if it can, and then resuming on its path.
        //needs to not go to the wall if near one. 
        public override void OnHit(GameObject _obj)
        {
            if (_obj.GetEntityType() == OBJECT_TYPE.Player)
            {
                DetermineDonation((Player)_obj);
            }
        }

        //get/set for donation stats. 
        public double GetDonationChance()
        { return donationChance; }
        public void ModifyDonationChance(double _mod)
        { donationChance += _mod; }

        //have they donated?
        public bool GetHasDonated()
        { return hasDonated; }
        //will they donate? 
        public bool GetWillDonate()
        { return willDonate; }


        public void SetNorthTile(TILE_TYPE _new)
        { tileToNorth = _new; }
        public TILE_TYPE GetNorthTile()
        { return tileToNorth; }
    }
}

//KW
