using Eyerpheus.Chests;
using Eyerpheus.Controllers.Eyetracker;
using Eyerpheus.Controllers.Instruments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyerpheus.Controllers.PlayModes
{
    public class PlayWithOneEye : IPlayMode, IBlinkerListener
    {

        public void receive_doubleClose()
        {
            
        }

        public void receive_doubleOpen()
        {

        }

        public void receive_leftClose()
        {
            InstrumentChest.getChest().Instrument.processStartPlaying();
        }

        public void receive_leftOpen()
        {
            InstrumentChest.getChest().Instrument.processStopPlaying();
        }

        public void receive_rightClose()
        {

        }

        public void receive_rightOpen()
        {

        }
    }
}
