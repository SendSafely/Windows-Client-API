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
        internal String DirectoryId
        {
            get { return directoryId; }
            set { directoryId = value; }
        }
        [JsonProperty(PropertyName = "name")]
        internal String Name
        {
            get { return name; }
            set { name = value; }
        }
        [JsonProperty(PropertyName = "created")]
        internal DateTime Created
        {
            get { return created; }
            set { created = value; }
        }
        [JsonProperty(PropertyName = "subDirectories")]
        internal ICollection<DirectoryResponse> SubDirectories
        {
            get { return subDirectories; }
            set { subDirectories = value; }
        }
    }
}
