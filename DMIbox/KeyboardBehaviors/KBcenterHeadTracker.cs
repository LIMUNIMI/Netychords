using NeeqDMIs.Eyetracking.PointFilters;
using NeeqDMIs.Eyetracking.Utils;
using NeeqDMIs.Keyboard;
using RawInputProcessor;

namespace Netychords
{
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