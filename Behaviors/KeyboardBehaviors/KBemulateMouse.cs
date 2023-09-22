using NITHdmis.Eyetracking.MouseEmulator;
using NITHdmis.Eyetracking.PointFilters;
using NITHdmis.Keyboard;
using RawInputProcessor;
using System.Windows.Input;

namespace Netychords.Behaviors.KeyboardBehaviors
{
    public class KBemulateMouse : IKeyboardBehavior
    {
        private VKeyCodes keyAction = VKeyCodes.Q;

        public int ReceiveEvent(RawInputEventArgs e)
        {
            if (e.VirtualKey == (ushort)keyAction)
            {
                R.NDB.TobiiModule.MouseEmulator = new MouseEmulatorModule(new PointFilterBypass());
                R.NDB.TobiiModule.MouseEmulator.Enabled = true;
                R.NDB.MainWindow.Cursor = Cursors.None;

                return 0;
            }

            return 1;
        }
    }
}