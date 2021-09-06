using Eyerpheus;
using Eyerpheus.Chests;
using Eyerpheus.Controllers.Eyetracker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eyerpheus.Controllers.PlayModes
{
    class RepeatNoteWithDouble : IBlinkerListener
    {
        public void receive_doubleClose()
        {
            InstrumentChest.getChest().Instrument.setNote(InstrumentChest.getChest().Note);
            GraphicsChest.getChest().NetytarDrawer.flashSpark();
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
