using System;
using System.Collections.Generic;
using System.IO;
using BillingManagement.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BillingManagement.Utils
{
    public class BillJsonReaderWriter : IBillReaderWriter
    {
        public BillJsonReaderWriter(string dataFolder)
        {
            _dataFolder = dataFolder;
        }

        public IList<BillInfo> GetAllBills()
        {
            IList<BillInfo> bills = new List<BillInfo>();
            if (!Directory.Exists(_dataFolder))
                return bills;

            foreach(string fileName in Directory.GetFiles(_dataFolder))
            {
                try
                {
                    string jsonString = File.ReadAllText(fileName);
                    JsonSerializer jsonSerializer = GetConfiguredJsonSerializer();
                    JsonReader textReader = new JsonTextReader(new StringReader(jsonString));
                    bills.Add(jsonSerializer.Deserialize<BillInfo>(textReader));
                }
                catch(System.Exception)
                {
                    continue;
                }
            }

            return bills;
        }

        public bool Record(BillInfo bill, out string errorString)
        {
            if(string.IsNullOrEmpty(bill.BillName))
            {
                errorString = "Bill name cannot be null or empty";
                return false;
            }

            Directory.CreateDirectory(_dataFolder);

            string filePath = $@".\{_dataFolder}\{bill.BillName}";

            if(File.Exists(filePath))
            {
                errorString = "Bill name is already existent. Please choose another name";
                return false;
            }

            JsonSerializer jsonSerializer = GetConfiguredJsonSerializer();
            using (StreamWriter sw = new StreamWriter(filePath) )
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                jsonSerializer.Serialize(writer, bill);
            }

            errorString = "";
            return true;
        }

        private JsonSerializer GetConfiguredJsonSerializer()
        {
            JsonSerializer jsonSerializer = new JsonSerializer();
            jsonSerializer.Converters.Add(new JavaScriptDateTimeConverter());
            jsonSerializer.NullValueHandling = NullValueHandling.Ignore;
            return jsonSerializer;
        }

        public IList<BillInfo> GetBillByFilter(Func<BillInfo, bool> filter)
        {
            throw new NotImplementedException();
        }

        private string _dataFolder = "";
    }
}
