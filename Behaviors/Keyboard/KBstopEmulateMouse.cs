using System.Windows.Input;
using Netychords.Modules;
using NITHdmis.Modules.Keyboard;
using RawInputProcessor;

namespace Netychords.Behaviors.Keyboard
{
    public class KBstopEmulateMouse : IKeyboardBehavior
    {
        private VKeyCodes keyAction = VKeyCodes.A;

        public int ReceiveEvent(RawInputEventArgs e)
        {
            if (e.VirtualKey == (ushort)keyAction)
            {
                Rack.BehaviorGazeToMouse.Enabled = false;
                Rack.MainWindow.Cursor = Cursors.Arrow;

                return 0;
            }

            return 1;
        }
    }
}