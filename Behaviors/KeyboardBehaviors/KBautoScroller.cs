using NITHdmis.Eyetracking.PointFilters;
using NITHdmis.Eyetracking.Utils;
using NITHdmis.Keyboard;
using RawInputProcessor;

namespace Netychords.Behaviors.KeyboardBehaviors
{
    public class KBautoScroller : IKeyboardBehavior
    {
        const VKeyCodes keyAction = VKeyCodes.W;

        public int ReceiveEvent(RawInputEventArgs e)
        {
            if (e.VirtualKey == (ushort)keyAction)
            {
                SetStuff();

                return 0;
            }

            return 1;
        }

        private void SetStuff()
        {
            R.NDB.AutoScroller.Enabled = true;
        }
    }
}