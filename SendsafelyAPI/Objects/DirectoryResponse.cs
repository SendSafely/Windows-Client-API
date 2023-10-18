using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DirectoryResponse
    {
        private String directoryId;
        private String name;
        private DateTime created;
        private ICollection<DirectoryResponse> subDirectories = new Collection<DirectoryResponse>();

        [JsonProperty(PropertyName = "directoryId")]
        public String DirectoryId
        {
            get { return directoryId; }
            set { directoryId = value; }
        }
        [JsonProperty(PropertyName = "name")]
        public String Name
        {
            get { return name; }
            set { name = value; }
        }
        [JsonProperty(PropertyName = "created")]
        public DateTime Created
        {
            get { return created; }
            set { created = value; }
        }
        [JsonProperty(PropertyName = "subDirectories")]
        public ICollection<DirectoryResponse> SubDirectories
        {
            get { return subDirectories; }
            set { subDirectories = value; }
        }
    }
}
