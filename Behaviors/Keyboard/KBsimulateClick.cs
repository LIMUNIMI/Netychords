using System.Windows;
using System.Windows.Controls.Primitives;
using Netychords.Modules;
using NITHdmis.Modules.Keyboard;
using RawInputProcessor;

namespace Netychords.Behaviors.Keyboard
{
    public class KBsimulateClick : IKeyboardBehavior
    {
        const VKeyCodes keyAction = VKeyCodes.C;
        bool one = false;

        public int ReceiveEvent(RawInputEventArgs e)
        {
            if (e is {VirtualKey: (ushort)keyAction, KeyPressState: KeyPressState.Down})
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
            if(Rack.MainWindow.LastSettingsGazedButton!= null)
            {
                Rack.MainWindow.LastSettingsGazedButton.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
            }
        }
    }
}