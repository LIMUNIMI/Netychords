using NITHdmis.Eyetracking.PointFilters;
using NITHdmis.Eyetracking.Utils;
using NITHdmis.Keyboard;
using RawInputProcessor;

namespace Netychords.Behaviors.KeyboardBehaviors
{
    public class KBsmute : IKeyboardBehavior
    {
        private VKeyCodes keyAction = VKeyCodes.N;

        public int ReceiveEvent(RawInputEventArgs e)
        {
            if (e.VirtualKey == (ushort)keyAction)
            {
                R.NDB.Mute = false;
            }

            return 1;
        }
    }
}