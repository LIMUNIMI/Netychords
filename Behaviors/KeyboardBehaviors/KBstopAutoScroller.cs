using NITHdmis.Eyetracking.PointFilters;
using NITHdmis.Eyetracking.Utils;
using NITHdmis.Keyboard;
using RawInputProcessor;

namespace Netychords.Behaviors.KeyboardBehaviors
{
    public class KBstopAutoScroller : IKeyboardBehavior
    {
        const VKeyCodes keyAction = VKeyCodes.S;

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
            R.NDB.AutoScroller.Enabled = false;
        }
    }
}