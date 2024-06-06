using Netychords.Modules;
using NITHdmis.Modules.Keyboard;
using RawInputProcessor;

namespace Netychords.Behaviors.Keyboard
{
    class KBplayStop : IKeyboardBehavior
    {
        private const VKeyCodes space = VKeyCodes.Space;
        private bool isDown = false;

        public int ReceiveEvent(RawInputEventArgs e)
        {
            if (Rack.MappingModule.keyboardEmulator)
            {
                if (e.VirtualKey == (ushort)space && e.KeyPressState == KeyPressState.Down && !isDown)
                {
                    Rack.MappingModule.PlayKeyDown = true;
                    isDown = true;
                    return 0;
                }
                if (e.VirtualKey == (ushort)space && e.KeyPressState == KeyPressState.Up)
                {
                    Rack.MappingModule.PlayKeyDown = false;
                    isDown = false;
                    return 0;
                };
            }
                
            return 1;
        }
    }
}
