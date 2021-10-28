using NeeqDMIs.Eyetracking.PointFilters;
using NeeqDMIs.Eyetracking.Utils;
using NeeqDMIs.Keyboard;
using RawInputProcessor;

namespace Netychords
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