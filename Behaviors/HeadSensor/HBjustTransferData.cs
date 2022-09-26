using NeeqDMIs.Headtracking.NeeqHT;

namespace Netychords.Behaviors.HeadSensor
{
    public class HBjustTransferData : INeeqHTbehavior
    {
        public void ReceiveHeadTrackerData(HeadTrackerData headTrackerData)
        {
            R.NDB.HTData = headTrackerData;
            R.NDB.ElaborateStrumming();
        }
    }
}
