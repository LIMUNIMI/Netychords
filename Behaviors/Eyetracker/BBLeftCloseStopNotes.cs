using Netychords.Modules;
using NITHlibrary.Nith.Preprocessors;

namespace Netychords.Behaviors.Eyetracker
{
    public class BBLeftCloseStopNotes : ANithBlinkEventBehavior
    {
        public BBLeftCloseStopNotes() : base()
        {
            this.LCThresh = 15;
        }
        protected override void Event_doubleClose()
        {

        }

        protected override void Event_doubleOpen()
        {
        }

        protected override void Event_leftClose()
        {
            if (!Rack.UserSettings.BlinkLeftStop)
                Rack.MappingModule.StopNotes();
        }

        protected override void Event_leftOpen()
        {
        }

        protected override void Event_rightClose()
        {
        }

        protected override void Event_rightOpen()
        {
        }
    }
}