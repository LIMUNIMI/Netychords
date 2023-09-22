using NITHdmis.Eyetracking.Tobii;
using NITHdmis.Mouse;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Netychords.Behaviors.Eyetracker
{
    class BBDoubleCloseClick : ATobiiBlinkBehavior
    {
        public BBDoubleCloseClick() : base()
        {
            DCThresh = 5;
        }
        public override void Event_doubleClose()
        {
            //MessageBox.Show(R.NDB.MainWindow.LastSettingsGazedButton.ToString());
            if (R.NDB.MainWindow.LastSettingsGazedButton != null)
            {
                //MessageBox.Show("Im In!");
                R.RaiseClickEvent = true;
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
