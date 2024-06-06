using NITHlibrary.Tools.Filters.PointFilters;
using System.Drawing;
using System.Windows.Controls;
using Netychords.Modules;

namespace Netychords.Surface
{
    /// <summary>
    /// Automatically scrolls a ScrollViewer following the mouse.
    /// </summary>
    public class AutoScroller_ButtonFollower : AutoScroller
    {
        public AutoScroller_ButtonFollower(ScrollViewer scrollViewer, int radiusThreshold, int proportional, IPointFilter filter) : base(scrollViewer, radiusThreshold, proportional, filter)
        {
            this.radiusThreshold = radiusThreshold;
            this.filter = filter;
            this.scrollViewer = scrollViewer;
            this.proportional = proportional;

            // Setting scrollviewer dimensions
            lastSampledPoint = new Point();
            basePosition = scrollViewer.PointToScreen(new System.Windows.Point(0, 0));
            scrollCenter = new System.Windows.Point(scrollViewer.ActualWidth / 2, scrollViewer.ActualHeight / 2);

            // Setting sampling timer
            samplerTimer.Interval = new TimeSpan(10000);//1000; //1;
            //samplerTimer.MicroTimerElapsed += SamplerTimer_MicroTimerElapsed;
            samplerTimer.Tick += ListenPosition;
            samplerTimer.Start();
        }

        protected new void ListenPosition(object sender, EventArgs e)
        {

            if (Enabled && Rack.Surface.CheckedButton != null)
            {
                var buttonX = Canvas.GetLeft(Rack.Surface.CheckedButton);
                var buttonY = Canvas.GetTop(Rack.Surface.CheckedButton);

                var differenceX = (buttonX - scrollViewer.HorizontalOffset - scrollCenter.X);
                var differenceY = (buttonY - scrollViewer.VerticalOffset - scrollCenter.Y);

                if (Math.Abs(differenceX) > radiusThreshold && Math.Abs(differenceY) > radiusThreshold && differenceY != 0 && differenceX != 0)
                {
                    scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset + (Math.Abs(Math.Pow((differenceX / proportional), 3))) * Math.Sign(differenceX));
                    scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + (Math.Abs(Math.Pow((differenceY / proportional), 3))) * Math.Sign(differenceY));
                }
            }
        }
    }
}
