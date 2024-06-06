using Netychords.Modules;
using NITHdmis.Modules.Keyboard;
using RawInputProcessor;

namespace Netychords.Behaviors.Keyboard
{
    public class KBsmute : IKeyboardBehavior
    {
        private VKeyCodes keyAction = VKeyCodes.N;

        public int ReceiveEvent(RawInputEventArgs e)
        {
            if (e.VirtualKey == (ushort)keyAction)
            {
                Rack.MappingModule.Mute = false;
            }

            return 1;
        }
    }
}