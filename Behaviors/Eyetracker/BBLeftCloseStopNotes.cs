using NeeqDMIs.Eyetracking.Tobii;

namespace Netychords.Behaviors.Eyetracker
{
    public class BBLeftCloseStopNotes : ATobiiBlinkBehavior
    {
        public BBLeftCloseStopNotes() : base()
        {
            this.LCThresh = 15;
        }
        public override void Event_doubleClose()
        {

        }

        public override void Event_doubleOpen()
        {
        }

        public override void Event_leftClose()
        {
            if (!R.UserSettings.BlinkPlay)
                R.NDB.StopNotes();
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