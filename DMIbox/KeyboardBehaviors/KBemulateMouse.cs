using NeeqDMIs.Eyetracking.PointFilters;
using NeeqDMIs.Eyetracking.Utils;
using NeeqDMIs.Keyboard;
using RawInputProcessor;

namespace Netychords
{
    public class KBemulateMouse : IKeyboardBehavior
    {
        private VKeyCodes keyAction = VKeyCodes.Q;

        public int ReceiveEvent(RawInputEventArgs e)
        {
            if (e.VirtualKey == (ushort)keyAction)
            {
                R.NDB.TobiiModule.MouseEmulator = new MouseEmulator(new PointFilterBypass());
                R.NDB.TobiiModule.MouseEmulator.EyetrackerToMouse = true;
                R.NDB.TobiiModule.MouseEmulator.CursorVisible = false;

                return 0;
            }

            return 1;
        }
    }
}