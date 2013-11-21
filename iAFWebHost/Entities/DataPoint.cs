using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iAFWebHost.Utils;

namespace iAFWebHost.Entities
{
    [Serializable]
    public class DataPoint : EntityBase
    {
        public DataPoint()
        {
            UtcTimeStamp = DateTime.UtcNow;
        }

        public DataPoint(string shortId)
        {
            UtcTimeStamp = DateTime.UtcNow;
            ShortId = shortId;
        }

        private string _id;
        public override string Id { get { return BuildKey(); } set { _id = value; } }
        public ulong Value { get; set; }
        public long Sum { get; set; }
        public long Min { get; set; }
        public long Max { get; set; }
        public long Count { get; set; }
        public long SumSqr { get; set; }

        [JsonIgnore]
        public override string Type { get { return "hit"; } }
        [JsonIgnore]
        public DateTime UtcTimeStamp { get; set; }
        [JsonIgnore]
        public string ShortId { get; set; }
        [JsonIgnore]
        public int Year { get { return UtcTimeStamp.Year; } }
        [JsonIgnore]
        public int Month { get { return UtcTimeStamp.Month; } }
        [JsonIgnore]
        public int Day { get { return UtcTimeStamp.Day; } }
        [JsonIgnore]
        public int Hour { get { return UtcTimeStamp.Hour; } }

        public string BuildKey()
        {
            if (!String.IsNullOrEmpty(_id))
                return _id;

            if (!ShortId.IsShortCode())
                throw new ArgumentNullException("ShortId can not be null");

            return String.Format("{0}_{1}_{2}_{3}_{4}_{5}", Type, ShortId, Year, Month, Day, Hour);
        }

        public bool ParseKey()
        {
            bool parsed = false;
            if (String.IsNullOrEmpty(_id))
                throw new ArgumentNullException("Id can not be null");

            string[] dataArray = _id.Split('_');
            if (dataArray.Length == 6)
            {
                int year, month, day, hour;
                if (Int32.TryParse(dataArray[2], out year)
                    && Int32.TryParse(dataArray[3], out month)
                    && Int32.TryParse(dataArray[4], out day)
                    && Int32.TryParse(dataArray[5], out hour))
                {
                    UtcTimeStamp = new DateTime(year, month, day, hour, 0, 0);
                    ShortId = dataArray[1];
                    parsed = true;
                }
            }
            return parsed;
        }
    }
}