﻿using Netychords.Modules;
using NITHdmis.Modules.Keyboard;
using RawInputProcessor;

namespace Netychords.Behaviors.Keyboard
{
    public class KBstopAutoScroller : IKeyboardBehavior
    {
        const VKeyCodes keyAction = VKeyCodes.S;

        public int ReceiveEvent(RawInputEventArgs e)
        {
            if (e.VirtualKey == (ushort)keyAction)
            {
                SetStuff();

                return 0;
            }

            return 1;
        }

        private void SetStuff()
        {
            Rack.AutoScroller.Enabled = false;
        }
    }
}