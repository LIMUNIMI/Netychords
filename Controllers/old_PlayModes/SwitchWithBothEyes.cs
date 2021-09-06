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
    public class SwitchWithBothEyes : IPlayMode, IBlinkerListener
    {
        private bool autoBlower = false;

        public void receive_doubleClose()
        {
            if (autoBlower)
            {
                autoBlower = false;
                InstrumentChest.getChest().Instrument.processStopPlaying();
            }
            else
            {
                autoBlower = true;
                InstrumentChest.getChest().Instrument.processStartPlaying();
            }
        }

        public void receive_doubleOpen()
        {

        }

        public void receive_leftClose()
        {

        }

        public void receive_leftOpen()
        {

        }

        public void receive_rightClose()
        {
  
        }

        public void receive_rightOpen()
        {

        }
    }
}
