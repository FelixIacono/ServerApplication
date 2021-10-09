using CsvHelper;
using CsvHelper.Configuration;
using Domain;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Services
{
    public class CsvHandler
    {
        private readonly CsvConfiguration _config;

        private readonly static string CsvPath = "C:\\Users\\User\\source\\repos\\ServerSideApplication\\Data\\data.csv";

        public CsvHandler()
        {
            _config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            };
        }

        public List<UserData> GetRecords()
        {
            using var reader = new StreamReader(CsvPath);
            using var csv = new CsvReader(reader, _config);

            var records = csv.GetRecords<UserData>();

            return records.ToList();
        }

        public UserData GetRecord(Guid id)
        {
            var records = GetRecords();

            return records.FirstOrDefault(x => x.Id == id);
        }

        public List<UserData> AppendRecord(UserData userData)
        {
            var records = GetRecords();

            records = records.Append(userData).ToList();

            using var writer = new StreamWriter(CsvPath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            csv.WriteRecords(records);

            return records;
        }

        public List<UserData> DeleteRecord(Guid id)
        {
            var records = GetRecords();

            records.RemoveAll(x => x.Id == id);

            using var writer = new StreamWriter(CsvPath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            csv.WriteRecords(records);

            return records;
        }
    }
}
