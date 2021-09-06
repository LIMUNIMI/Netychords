using NeeqDMIs.Eyetracking.PointFilters;
using System;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Netychords
{
    /// <summary>
    /// Automatically scrolls a ScrollViewer following the mouse.
    /// </summary>
    public class AutoScroller
    {
        #region Params

        private ScrollViewer scrollViewer;
        private int radiusThreshold;
        private int proportional;
        private IPointFilter filter;

        #endregion Params

        #region Scrollviewer params

        private System.Windows.Point scrollCenter;
        private System.Windows.Point basePosition;

        #endregion Scrollviewer params

        #region Internal counters

        private DispatcherTimer samplerTimer = new DispatcherTimer(DispatcherPriority.Render);

        // private Timer samplerTimer = new Timer();
        // private MicroTimer samplerTimer = new MicroTimer();
        private Point lastSampledPoint;

        private Point lastMean;
        private double Xdifference;
        private double Ydifference;

        #endregion Internal counters


        private System.Windows.Point temp = new System.Windows.Point();

        public AutoScroller(ScrollViewer scrollViewer, int radiusThreshold, int proportional, IPointFilter filter)
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
            samplerTimer.Tick += ListenMouse;
            samplerTimer.Start();
        }

        public bool Enabled
        {
            get; set;
        } = false;

        private void ListenMouse(object sender, EventArgs e)
        {
            if (Enabled)
            {
                lastSampledPoint.X = GetMousePos().X - (int)basePosition.X;
                lastSampledPoint.Y = GetMousePos().Y - (int)basePosition.Y;

                filter.Push(lastSampledPoint);
                lastMean = filter.GetOutput();

                Scroll();
            }
        }

        private void Scroll()
        {
            Xdifference = (scrollCenter.X - lastMean.X);
            Ydifference = (scrollCenter.Y - lastMean.Y);
            if (Math.Abs(scrollCenter.Y - lastMean.Y) > radiusThreshold && Math.Abs(scrollCenter.X - lastMean.X) > radiusThreshold)
            {
                scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - Math.Pow((Xdifference / proportional), 2) * Math.Sign(Xdifference));
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - Math.Pow((Ydifference / proportional), 2) * Math.Sign(Ydifference));
            }
        }

        private Point GetMousePos()
        {
            temp = scrollViewer.PointToScreen(Mouse.GetPosition(scrollViewer));
            return new Point((int)temp.X, (int)temp.Y);
        }
        public void ScrollTo(int X, int Y)
        {
            scrollViewer.ScrollToVerticalOffset(Y / 2);
            scrollViewer.ScrollToHorizontalOffset(X / 2);
        }
    }
}