using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Netychords.Modules;
using Netychords.Utils;

namespace Netychords.Surface
{
    public class NetychordsButton : RadioButton
    {
        #region Internal params

        private MidiChord chord;
        private NetychordsSurface NetychordsDrawer;
        private Ellipse occluder;
        public MidiChord Chord { get { return chord; } set { chord = value; } }

        public Ellipse Occluder
        {
            get
            {
                return occluder;
            }

            set
            {
                occluder = value;
            }
        }

        #endregion Internal params

        public NetychordsButton(NetychordsSurface netychordsSurface) : base()
        {
            this.NetychordsDrawer = netychordsSurface;

            occluder = new Ellipse();
            occluder.Stroke = Brushes.Transparent;
            occluder.Fill = Brushes.Transparent;
            occluder.Stroke = new SolidColorBrush(Color.FromArgb(50, 0, 0, 0));
            occluder.Fill = new SolidColorBrush(Color.FromArgb(200, 255, 0, 0));
            occluder.StrokeThickness = 1;
            occluder.HorizontalAlignment = HorizontalAlignment.Center;
            occluder.VerticalAlignment = VerticalAlignment.Center;

            occluder.MouseEnter += OccluderMouseEnterBehavior;
            Visibility = Visibility.Hidden;
        }

        private void OccluderMouseEnterBehavior(object sender, MouseEventArgs e)
        {
            NetychordsDrawer.NetychordsButton_OccluderMouseEnter(this);
            Rack.MainWindow.Global_NetychordsButtonEntered();
        }
    }
}