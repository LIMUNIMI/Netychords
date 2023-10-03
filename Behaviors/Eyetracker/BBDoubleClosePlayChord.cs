using Netychords.Settings;
using NITHdmis.Eyetracking.Tobii;

namespace Netychords.Behaviors.Eyetracker
{
    class BBDoubleClosePlayChord : ATobiiBlinkBehavior
    {
        public BBDoubleClosePlayChord() : base()
        {
            this.DCThresh = 4;
        }
        public override void Event_doubleClose()
        {
            if (R.UserSettings.InteractionMethod == NetychordsInteractionMethod.PressureBlink || R.UserSettings.InteractionMethod == NetychordsInteractionMethod.Blink)
            {
                R.NDB.PlayChord(R.NDB.Chord);
            }
            
        }

        public override void Event_doubleOpen()
        {
        }

        public override void Event_leftClose()
        {
        }

        public override void Event_leftOpen()
        {
        }

        public override void Event_rightClose()
        {
        }

        public override void Event_rightOpen()
        {
        }
    }
}
