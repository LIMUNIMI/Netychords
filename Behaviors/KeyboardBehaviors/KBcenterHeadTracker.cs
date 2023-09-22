using NITHdmis.Eyetracking.PointFilters;
using NITHdmis.Eyetracking.Utils;
using NITHdmis.Keyboard;
using RawInputProcessor;

namespace Netychords.Behaviors.KeyboardBehaviors{
    public class KBcenterHeadTracker : IKeyboardBehavior
    {
        private VKeyCodes keyAction = VKeyCodes.C;

        public int ReceiveEvent(RawInputEventArgs e)
        {
            if (e.VirtualKey == (ushort)keyAction)
            {
                R.NDB.CalibrationHeadSensor();

                return 0;
            }

            return 1;
        }
    }
}