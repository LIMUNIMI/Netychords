using Netychords.Modules;
using NITHdmis.Modules.Keyboard;
using RawInputProcessor;

namespace Netychords.Behaviors.Keyboard
{
    public class KBautoStrum : IKeyboardBehavior
    {
        private const VKeyCodes keyStart = VKeyCodes.O;
        private const VKeyCodes keyStop = VKeyCodes.L;

        public int ReceiveEvent(RawInputEventArgs e)
        {
            if (e.VirtualKey == (ushort)keyStart)
            {
                Rack.UserSettings.AutoStrum = true;

                Rack.MappingModule.StartAutostrum(Rack.UserSettings.AutoStrumBPM);

                return 0;
            }
            if (e.VirtualKey == (ushort)keyStop)
            {
                Rack.UserSettings.AutoStrum = false;

                Rack.MappingModule.StopAutostrum();

                return 0;
            }

            return 1;
        }

        private void SetStuff()
        {
        }
    }
}