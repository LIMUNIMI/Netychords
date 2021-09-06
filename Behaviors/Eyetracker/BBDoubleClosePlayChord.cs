using NeeqDMIs.Eyetracking.Tobii;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if (R.UserSettings.BlinkPlay)
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
