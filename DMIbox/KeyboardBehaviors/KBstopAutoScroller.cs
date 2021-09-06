using NeeqDMIs.Eyetracking.PointFilters;
using NeeqDMIs.Eyetracking.Utils;
using NeeqDMIs.Keyboard;
using RawInputProcessor;

namespace Netychords
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