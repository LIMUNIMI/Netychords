using Eyerpheus;
using Eyerpheus.Chests;
using Eyerpheus.Controllers.Eyetracker;
using Eyerpheus.Controllers.Instruments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Eyerpheus.Controllers.Instruments
{
    class ScaleBlinker : IBlinkerListener
    {
        private string leftScaleCode;
        private string rightScaleCode;

        public ScaleBlinker(string leftScaleCode, string rightScaleCode)
        {
            this.rightScaleCode = rightScaleCode;
            this.leftScaleCode = leftScaleCode;
        }

        public void receive_doubleClose()
        {
            
        }

        public void receive_doubleOpen()
        {
            
        }

        public void receive_leftClose()
        {
            GraphicsChest.getChest().NetytarDrawer.drawLines(MusicMaster.getScaleEnum(InstrumentChest.getChest().Instrument.getNote(), leftScaleCode), Colors.Blue);
            GraphicsChest.getChest().NetytarDrawer.drawEllipses(MusicMaster.getScaleEnum(InstrumentChest.getChest().Instrument.getNote(), leftScaleCode));
        }

        public void receive_leftOpen()
        {
           
        }

        public void receive_rightClose()
        {
            GraphicsChest.getChest().NetytarDrawer.drawLines(MusicMaster.getScaleEnum(InstrumentChest.getChest().Instrument.getNote(), rightScaleCode), Colors.Red);
            GraphicsChest.getChest().NetytarDrawer.drawEllipses(MusicMaster.getScaleEnum(InstrumentChest.getChest().Instrument.getNote(), rightScaleCode));
        }

        public void receive_rightOpen()
        {

        }
    }
}
