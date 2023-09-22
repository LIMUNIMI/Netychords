using NITHdmis.Keyboard;
using RawInputProcessor;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace Netychords.Behaviors.KeyboardBehaviors
{
    public class KBsimulateClick : IKeyboardBehavior
    {
        const VKeyCodes keyAction = VKeyCodes.C;
        bool one = false;

        public int ReceiveEvent(RawInputEventArgs e)
        {
            if (e.VirtualKey == (ushort)keyAction && e.KeyPressState == KeyPressState.Down)
            {
                one = !one;
                if(one)
                DoStuff();

                return 0;
            }

            return 1;
        }

        private void DoStuff()
        {
            if(R.NDB.MainWindow.LastSettingsGazedButton!= null)
                R.NDB.MainWindow.LastSettingsGazedButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        }
    }
}