using NeeqDMIs.Eyetracking.PointFilters;
using NeeqDMIs.Eyetracking.Utils;
using NeeqDMIs.Keyboard;
using RawInputProcessor;

namespace Netychords
{
    public class KBstopEmulateMouse : IKeyboardBehavior
    {
        private VKeyCodes keyAction = VKeyCodes.A;

        public int ReceiveEvent(RawInputEventArgs e)
        {
            if (e.VirtualKey == (ushort)keyAction)
            {
                R.NDB.TobiiModule.MouseEmulator.EyetrackerToMouse = false;
                R.NDB.TobiiModule.MouseEmulator.CursorVisible = true;

                return 0;
            }

            return 1;
        }
    }
}