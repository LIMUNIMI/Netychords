using Netychords.Modules;
using NITHdmis.Modules.Keyboard;
using RawInputProcessor;

namespace Netychords.Behaviors.Keyboard
{
    public class KBcenterHeadTracker : IKeyboardBehavior
    {
        private VKeyCodes keyAction = VKeyCodes.C;

        public int ReceiveEvent(RawInputEventArgs e)
        {
            if (e.VirtualKey == (ushort)keyAction)
            {
                Rack.PreprocessorHeadTrackerCalibrator.SetCenterToCurrentPosition();

                return 0;
            }

            return 1;
        }
    }
}