using Netychords.Utils;

namespace Netychords.Modules.CustomRows
{
    public class CustomRow
    {
        public CustomRow()
        {

        }
        public CustomRow(int id, bool enabled = false, ChordType chordType = ChordType.Maj, int shift = 0)
        {
            Id = id;
            Enabled = enabled;
            ChordType = chordType;
            Shift = shift;
        }

        public int Id { get; set; }
        public bool Enabled { get; set; }
        public ChordType ChordType { get; set; }
        public int Shift { get; set; }
    }
}