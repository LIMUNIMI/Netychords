using System.Drawing;
using System.Windows.Controls;
using System.Windows.Media;
using Netychords.Modules;

namespace Netychords.Surface.FlowerLayout
{
    public class FlowerButton : NetychordsButton
    {
        public static int DimButton = 10;
        public static int DimOccluder = 40;
        private const int ZIndexButton = 30;
        private const int ZIndexOccluder = 2;
        public Point Coordinates { get; set; }

        public FlowerButton(Point coordinates) : base(Rack.Surface)
        {
            Coordinates = coordinates;

            Panel.SetZIndex(this, ZIndexButton);
            Panel.SetZIndex(this.Occluder, ZIndexOccluder);
            Occluder.Width = DimOccluder;
            Occluder.Height = DimOccluder;
            Height = DimButton;
            Width = DimButton;
            Occluder.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromArgb(10, 0, 0, 0));
        }

        public void SetColor(SolidColorBrush solidColorBrush)
        {
            Occluder.Fill = solidColorBrush;
        }
    }
}