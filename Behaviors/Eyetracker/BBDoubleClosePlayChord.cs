using Netychords.Modules;
using Netychords.Settings;
using NITHlibrary.Nith.Behaviors;

namespace Netychords.Behaviors.Eyetracker
{
    class BBDoubleClosePlayChord : ANithBlinkEventBehavior
    {
        public BBDoubleClosePlayChord()
        {
            DCThresh = 2;
        }
        protected override void Event_doubleClose()
        {
            if (Rack.UserSettings.InteractionMethod == NetychordsInteractionMethod.PressureBlink || Rack.UserSettings.InteractionMethod == NetychordsInteractionMethod.Blink)
            {
                Rack.MappingModule.StopNotes();
                Rack.MappingModule.PlayChord(Rack.MappingModule.Chord);
            }
            
        }

        protected override void Event_doubleOpen()
        {
        }

        protected override void Event_leftClose()
        {
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
