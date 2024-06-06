using System.Windows.Input;
using Netychords.Modules;
using NITHdmis.Modules.Keyboard;
using RawInputProcessor;

namespace Netychords.Behaviors.Keyboard
{
    public class KBemulateMouse : IKeyboardBehavior
    {
        private VKeyCodes keyAction = VKeyCodes.Q;

        public int ReceiveEvent(RawInputEventArgs e)
        {
            if (e.VirtualKey == (ushort)keyAction)
            {
                Rack.BehaviorGazeToMouse.Enabled = true;
                Rack.MainWindow.Cursor = Cursors.None;

                return 0;
            }

            return 1;
        }
    }
}