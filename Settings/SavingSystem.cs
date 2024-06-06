using System.Globalization;
using System.IO;
using CsvHelper;
using Netychords.Modules.CustomRows;

namespace Netychords.Settings
{
    public class SavingSystem
    {
        public const string SETTINGSFILENAME = "Settings";
        public const string CUSTOMROWSFILENAME = "CustomRows";

        public SavingSystem(string settingsFilename = SETTINGSFILENAME, string customRowsFilename = CUSTOMROWSFILENAME)
        {
            SettingsFilename = settingsFilename;
            CustomRowsFilename = customRowsFilename;
        }

        private string SettingsFilename { get; set; }
        private string CustomRowsFilename { get; set; }

        public int SaveCustomRows(List<CustomRow> customRows)
        {
            try
            {
                using (var writer = new StreamWriter(CustomRowsFilename + ".csv"))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(customRows);
                }
                return 0;
            }
            catch
            {
                return 1;
            }
        }

        public int SaveSettings(NetychordsSettings settings)
        {
            try
            {
                // Settings

                List<NetychordsSettings> records = new List<NetychordsSettings> { settings };

                using (var writer = new StreamWriter(SettingsFilename + ".csv"))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(records);
                }

                return 0;
            }
            catch
            {
                return 1;
            }
        }

        public NetychordsSettings LoadSettings()
        {
            try
            {
                List<NetychordsSettings> settings = new List<NetychordsSettings>();
                using (var reader = new StreamReader(SettingsFilename + ".csv"))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<NetychordsSettings>();
                    settings = records.ToList();
                }
                return settings[0];
            }
            catch
            {
                return new DefaultSettings();
            }
        }

        public List<CustomRow> LoadCustomRows()
        {
            List<CustomRow> rows = new List<CustomRow>
            {
                new CustomRow(1),
                new CustomRow(2),
                new CustomRow(3),
                new CustomRow(4),
                new CustomRow(5),
                new CustomRow(6),
                new CustomRow(7),
                new CustomRow(8),
                new CustomRow(9)
            };

            try
            {
                using (var reader = new StreamReader(CustomRowsFilename + ".csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<CustomRow>();
                rows = records.ToList();
                return rows;
            }
            }
            catch
            {
                return rows;
            }
        }
    }
}