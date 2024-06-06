namespace Netychords.Modules.CustomRows
{
    internal class CustomRowsManager
    {
        public CustomRowsManager()
        {
            CR1 = new CustomRow(1);
            CR2 = new CustomRow(2);
            CR3 = new CustomRow(3);
            CR4 = new CustomRow(4);
            CR5 = new CustomRow(5);
            CR6 = new CustomRow(6);
            CR7 = new CustomRow(7);
            CR8 = new CustomRow(8);
            CR9 = new CustomRow(9);
        }
        public CustomRow CR1 { get; set; }
        public CustomRow CR2 { get; set; }
        public CustomRow CR3 { get; set; }
        public CustomRow CR4 { get; set; }
        public CustomRow CR5 { get; set; }
        public CustomRow CR6 { get; set; }
        public CustomRow CR7 { get; set; }
        public CustomRow CR8 { get; set; }
        public CustomRow CR9 { get; set; }

        internal List<CustomRow> GetRows()
        {
            return new List<CustomRow>
            {
                CR1, CR2, CR3, CR4, CR5, CR6, CR7, CR8, CR9
            };
        }

        internal void SetRows(List<CustomRow> customRows)
        {
            CR1 = customRows[0];
            CR2 = customRows[1];
            CR3 = customRows[2];
            CR4 = customRows[3];
            CR5 = customRows[4];
            CR6 = customRows[5];
            CR7 = customRows[6];
            CR8 = customRows[7];
            CR9 = customRows[8];
        }
    }
}
