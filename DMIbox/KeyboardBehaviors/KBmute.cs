using NeeqDMIs.Eyetracking.PointFilters;
using NeeqDMIs.Eyetracking.Utils;
using NeeqDMIs.Keyboard;
using RawInputProcessor;

namespace Netychords
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