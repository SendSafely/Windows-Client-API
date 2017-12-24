using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class GetDirectoryResponse : StandardResponse
    {
        private Directory directory;
        private String directoryName;
        private String directoryId;
        private List<FileResponse> files;
        private Collection<DirectoryResponse> subDirectories;

        [JsonProperty(PropertyName = "directory")]
        internal Directory Directory
        {
            get { return directory; }
            set { directory = value; }
        }
        [JsonProperty(PropertyName = "directoryName")]
        internal String DirectoryName
        {
            get { return directoryName; }
            set { directoryName = value; }
        }
        [JsonProperty(PropertyName = "directoryId")]
        internal String DirectoryId
        {
            get { return directoryId; }
            set { directoryId = value; }
        }
        [JsonProperty(PropertyName = "files")]
        internal List<FileResponse> Files
        {
            get { return files; }
            set { files = value; }
        }
        [JsonProperty(PropertyName = "subdirectories")]
        internal Collection<DirectoryResponse> SubDirectories
        {
            get { return subDirectories; }
            set { subDirectories = value; }
        }
    }
}
