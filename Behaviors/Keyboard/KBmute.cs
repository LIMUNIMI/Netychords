using Netychords.Modules;
using NITHdmis.Modules.Keyboard;
using RawInputProcessor;

namespace Netychords.Behaviors.Keyboard
{
    public class KBmute : IKeyboardBehavior
    {
        private VKeyCodes keyAction = VKeyCodes.M;

        public int ReceiveEvent(RawInputEventArgs e)
        {
            if (e.VirtualKey == (ushort)keyAction)
            {
                Rack.MappingModule.Mute = true;
            }

            return 1;
        }
    }
}