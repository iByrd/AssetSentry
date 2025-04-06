using Azure.Core.GeoJson;
using System.ComponentModel;

namespace AssetSentry.Models
{
    public class Log
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public ObjectType ObjectType { get; set; }

        public Action Action { get; set; }

        public int ObjectId { get; set; }

        public DateTime LogTime { get; set; }
    }
}
