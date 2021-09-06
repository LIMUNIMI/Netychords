using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using Eyerpheus.Controllers.Music;

namespace Netychords
{
    class RhythmFlasher
    {
        private ScrollViewer scrollViewer;
        private Timer beatTimer = new Timer();
        private Timer changerTimer = new Timer();

        private int changeTempo = 10;
        private byte changerOffset = 2;
        private Color baseColor = Colors.Beige;
        private SolidColorBrush colorBrush = new SolidColorBrush();
        private Brush imageBrushBackup;
        private bool started = false;

        private const int maxBpm = 240;
        private const int minBpm = 40;
        private const int defaultBpm = 120;

        public int Bpm
        {
            get
            {
                return TempoUtils.MillisecondsToBpm(beatTimer.Interval);
            }

            set
            {
                if (value >= minBpm && value <= maxBpm)
                {
                    beatTimer.Interval = TempoUtils.BpmToMilliseconds(value);
                }
                
            }
        }

        public RhythmFlasher(ScrollViewer scrollViewer)
        {
            this.scrollViewer = scrollViewer;
            colorBrush.Color = baseColor;
            imageBrushBackup = scrollViewer.Background;

            beatTimer.Tick += beatTimer_Tick;
            changerTimer.Tick += changerTimer_Tick;
            beatTimer.Interval = 1000;
            changerTimer.Interval = changeTempo;
        }

        public RhythmFlasher(ScrollViewer scrollViewer, Color baseColor)
        {
            this.scrollViewer = scrollViewer;
            this.baseColor = baseColor;
            colorBrush.Color = baseColor;
            imageBrushBackup = scrollViewer.Background;

            beatTimer.Tick += beatTimer_Tick;
            changerTimer.Tick += changerTimer_Tick;
            beatTimer.Interval = 1000;
            changerTimer.Interval = changeTempo;
        }

        private void changerTimer_Tick(object sender, EventArgs e)
        {
            colorBrush.Color = Color.FromRgb((byte)(colorBrush.Color.R - changerOffset), (byte)(colorBrush.Color.G - changerOffset), (byte)(colorBrush.Color.B - changerOffset));
            scrollViewer.Background = colorBrush;
        }

        private void beatTimer_Tick(object sender, EventArgs e)
        {
            colorBrush.Color = baseColor;
            scrollViewer.Background = colorBrush;
        }

        public void start()
        {
            beatTimer.Start();
            changerTimer.Start();
            started = true;
        }

        public void stop()
        {
            beatTimer.Stop();
            changerTimer.Stop();
            colorBrush.Color = baseColor;
            scrollViewer.Background = imageBrushBackup;
            started = false;
        }

        public bool isStarted()
        {
            return started;
        }
        
    }
}
