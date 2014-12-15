//style from game state management samples.
//http://xbox.create.msdn.com/en-US/education/catalog/sample/game_state_management

using System;
using System.IO;

//**KW -> not sure if want!
//#if WINDOWS || XBOX
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;
//#endif

namespace Captain_Of_Industry
{
    public enum TransitionState
    { TRANSITION_ON, ACTIVE, TRANSITION_OFF, HIDDEN }

    //is implemented by various game states that could happen.
    //main menu, game, endgame, etc.
    public abstract class GameScreen
    {
        //assumes no fade for now; can be modified to a set interval, as we choose.
        private TimeSpan transitionTime = TimeSpan.Zero;
        //allows for transitions between screens -> fades, slides?
        //assumes we want equal fade to and from a screen.
        public TimeSpan TransitionTime
        {
            get
            { return transitionTime; }
            set
            { transitionTime = value; }
        }

        //keeps track of slide
        private float transitionPos;
        public float TransitionPosition
        {
            get { return transitionPos; }
            protected set { transitionPos = value; }
        }

        //whether or not this screen has transitions and if so, where it is.
        private TransitionState currentState = TransitionState.TRANSITION_ON;
        public TransitionState TransitionState
        {
            get
            { return currentState; }
            set
            { currentState = value; }
        }

        //check to see if the screen is done forever (when exiting game, for instance) 
        //or if it is just being covered by another (for pop-ups, pausing, etc.)
        private bool isExiting;
        public bool IsExiting
        {
            get
            { return isExiting; }
            internal set
            { isExiting = value; }
        }

        //is this currently the active screen, if being covered by another? 
        private bool isActive;
        public bool IsActive
        {
            get
            { return (!isActive && (currentState == TransitionState.TRANSITION_ON || currentState == TransitionState.ACTIVE)); }
        }

       //can the window be serialized? 
        private bool isSerializable = true;
        public bool IsSerializable
        {
            get
            { return isSerializable; }
            set
            { isSerializable = value; } 
        }

        //activates screen ->when added to to screenmanager or game resumes.
        public virtual void Active(bool _preserved) { } 
        //deactivates screen when pausing.
        public virtual void Deactivate() { }
        //releases resources/content from screen, when removed from manager.
        //--> effective destructor
        public virtual void Unload() { }

        //standard update, screen may run logic; not allowed during pause.
        public virtual void Update(GameTime _time, bool _otherScreenFocused, bool _isActiveScreen)
        {
            this.isActive = _isActiveScreen;

            if (isExiting)
            {
                currentState = TransitionState.TRANSITION_OFF;

                if (UpdateTransition(_time, transitionTime, 1))
                {
                    currentState = TransitionState.TRANSITION_OFF;
                }
                else //transition complete
                {
                    currentState = TransitionState.HIDDEN;
                }
            }
            else
            {
                if (UpdateTransition(_time, transitionTime, -1))
                {
                    currentState = TransitionState.TRANSITION_ON;
                }
                else
                {
                    currentState = TransitionState.ACTIVE;
                }
            }
        }
        //allows handling of input, only occurs when active.
        //** KW --> Placeholder inputstate, needs reference to instance of input manager!
        public virtual void HandleInput(GameTime _dt) { }
        //updates the sliiide between two screens.
        private bool UpdateTransition(GameTime _dt, TimeSpan _time, int _dir)
        {
            float tDelta;

            if (_time == TimeSpan.Zero)
            { tDelta = 1; }
            else
            { tDelta = (float)(_dt.ElapsedGameTime.TotalMilliseconds / _time.TotalMilliseconds); }

            transitionPos += tDelta * _dir;

            if (((_dir < 0) && (transitionPos <= 0)) || ((_dir > 0) && (transitionPos >= 1)))
            {
                transitionPos = MathHelper.Clamp(transitionPos, 0, 1);
                return false;
            }

            return true;
        }
        //exit screen via transition.
        public void ExitScreen()
        {
            isExiting = true;
            //will handle transition as needed from here. 
        }
        
        //rendering.
        public virtual void Render(GameTime _dt) { }
    }
}

//do we want to implement pop-ups as mini-game screens..?
//is popup, translation 

//KW.
