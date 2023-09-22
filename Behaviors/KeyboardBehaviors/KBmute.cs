using NITHdmis.Eyetracking.PointFilters;
using NITHdmis.Eyetracking.Utils;
using NITHdmis.Keyboard;
using RawInputProcessor;

namespace Netychords.Behaviors.KeyboardBehaviors
{
    public class KBmute : IKeyboardBehavior
    {
        private VKeyCodes keyAction = VKeyCodes.M;

        public int ReceiveEvent(RawInputEventArgs e)
        {
            if (e.VirtualKey == (ushort)keyAction)
            {
                R.NDB.Mute = true;
            }

            return 1;
        }
    }
}