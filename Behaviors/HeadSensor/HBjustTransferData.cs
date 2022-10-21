using NeeqDMIs.Headtracking.NeeqHT;

namespace Netychords.Behaviors.HeadSensor
{
    public class HBjustTransferData : INeeqHTbehavior
    {
        public void ReceiveHeadTrackerData(NeeqHTData headTrackerData)
        {
            R.NDB.HTData = headTrackerData;
            R.NDB.ElaborateStrumming();
        }
    }
}
