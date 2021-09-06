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
    public class HoldWithOneEye : IBlinkerListener
    {
        public void receive_doubleClose()
        {
            
        }

        public void receive_doubleOpen()
        {

        }

        public void receive_leftClose()
        {
            InstrumentChest.getChest().Instrument.processStartHold();
        }

        public void receive_leftOpen()
        {
            InstrumentChest.getChest().Instrument.processStopHold();
        }

        public void receive_rightClose()
        {

        }

        public void receive_rightOpen()
        {

        }
    }
}
