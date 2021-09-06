using NeeqDMIs.Keyboard;
using RawInputProcessor;

namespace Netychords
{
    public class KBautoStrum : IKeyboardBehavior
    {
        private const VKeyCodes keyStart = VKeyCodes.O;
        private const VKeyCodes keyStop = VKeyCodes.L;

        public int ReceiveEvent(RawInputEventArgs e)
        {
            if (e.VirtualKey == (ushort)keyStart)
            {
                R.UserSettings.AutoStrum = true;

                R.NDB.StartAutostrum(R.UserSettings.AutoStrumBPM);

                return 0;
            }
            if (e.VirtualKey == (ushort)keyStop)
            {
                R.UserSettings.AutoStrum = false;

                R.NDB.StopAutostrum();

                return 0;
            }

            return 1;
        }

        private void SetStuff()
        {
        }
    }
}