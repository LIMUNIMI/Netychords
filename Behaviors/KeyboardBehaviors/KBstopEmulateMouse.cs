using NITHdmis.Keyboard;
using RawInputProcessor;
using System.Windows.Input;

namespace Netychords.Behaviors.KeyboardBehaviors
{
    public class KBstopEmulateMouse : IKeyboardBehavior
    {
        private VKeyCodes keyAction = VKeyCodes.A;

        public int ReceiveEvent(RawInputEventArgs e)
        {
            if (e.VirtualKey == (ushort)keyAction)
            {
                R.NDB.TobiiModule.MouseEmulator.Enabled = false;
                R.NDB.MainWindow.Cursor = Cursors.Arrow;

                return 0;
            }

            return 1;
        }
    }
}