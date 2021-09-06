using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Eyerpheus.Controllers.Graphics
{
    class IndependentLineFlashTimer : System.Windows.Forms.Timer
    {
        private Line flashLine;
        private Canvas canvas;

        public IndependentLineFlashTimer(Point point1, Point point2, Canvas canvas, Color color)
        {
            this.canvas = canvas;
            flashLine = new Line();
            flashLine.X1 = point1.X;
            flashLine.X2 = point2.X;
            flashLine.Y1 = point1.Y;
            flashLine.Y2 = point2.Y;
            flashLine.StrokeThickness = 4;
            flashLine.HorizontalAlignment = HorizontalAlignment.Left;
            flashLine.VerticalAlignment = VerticalAlignment.Center;
            flashLine.Stroke = new SolidColorBrush(color);
            canvas.Children.Add(flashLine);
            Panel.SetZIndex(flashLine, 13);

            Interval = 20;
            Enabled = true;
           
            Tick += LineFlashTimer_Tick;

        }

        private void LineFlashTimer_Tick(object sender, EventArgs e)
        {
            flashLine.Opacity -= 0.05;
            if (flashLine.Opacity <= 0)
            {
                canvas.Children.Remove(flashLine);
                Dispose();
            } 
        }
    }
}
