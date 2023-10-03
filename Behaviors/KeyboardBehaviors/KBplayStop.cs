using NITHdmis.Keyboard;
using NITHdmis.Music;
using RawInputProcessor;

namespace Netychords.DMIBox.KeyboardBehaviors
{
    class KBplayStop : IKeyboardBehavior
    {
        private const VKeyCodes space = VKeyCodes.Space;
        private bool isDown = false;

        public int ReceiveEvent(RawInputEventArgs e)
        {
            if (R.NDB.keyboardEmulator)
            {
                if (e.VirtualKey == (ushort)space && e.KeyPressState == KeyPressState.Down && !isDown)
                {
                    R.NDB.PlayKeyDown = true;
                    isDown = true;
                    return 0;
                }
                if (e.VirtualKey == (ushort)space && e.KeyPressState == KeyPressState.Up)
                {
                    R.NDB.PlayKeyDown = false;
                    isDown = false;
                    return 0;
                };
            }
                
            return 1;
        }
    }
}
