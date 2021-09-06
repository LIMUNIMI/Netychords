using Eyerpheus.Chests;
using Eyerpheus.Controllers.Eyetracker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyerpheus.Controllers.PlayModes
{
    class PauseWithOneEye : IBlinkerListener
    {
        private Eyes eye;

        public PauseWithOneEye(Eyes eye)
        {
            this.eye = eye;
        }

        public void receive_doubleClose()
        {
            
        }

        public void receive_doubleOpen()
        { 

        }

        public void receive_leftClose()
        {
            if(eye == Eyes.Left)
            {
                startPause();
            }
        }

        private static void startPause()
        {
            InstrumentChest.getChest().Instrument.processStopPlaying();
        }

        private static void endPause()
        {
            InstrumentChest.getChest().Instrument.processStartPlaying();
        }

        public void receive_leftOpen()
        {
            if(eye == Eyes.Right)
            {
                endPause();
            }
        }

        public void receive_rightClose()
        {
            if(eye == Eyes.Left)
            {
                startPause();
            }
        }

        public void receive_rightOpen()
        {
            if(eye == Eyes.Right)
            {
                endPause();
            }
            
        }
    }
}
