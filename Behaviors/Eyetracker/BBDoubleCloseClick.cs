using Netychords.Modules;
using NITHlibrary.Nith.Preprocessors;

namespace Netychords.Behaviors.Eyetracker
{
    class BBDoubleCloseClick : ANithBlinkEventBehavior
    {
        public BBDoubleCloseClick()
        {
            DCThresh = 10;
        }
        protected override void Event_doubleClose()
        {
            if (Rack.MainWindow.LastSettingsGazedButton != null)
            {
                Rack.RaiseClickEvent = true;
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
