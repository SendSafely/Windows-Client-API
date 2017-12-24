using SendSafely.Objects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace SendSafely
{
    public class Directory
    {
        private Directory userDirectory;
        private String directoryName;
        private String directoryId;
        private List<FileResponse> files;
        private Collection<DirectoryResponse> subDirectories;

        public Directory()
        {
        }

        public Directory(String directoryId, String directoryName)
        {
            this.directoryId = directoryId;
            this.directoryName = directoryName;
        }

        public Directory UserDirectory
        {
            get { return userDirectory; }
            set { userDirectory = value; }
        }
        public String DirectoryName
        {
            get { return directoryName; }
            set { directoryName = value; }
        }
        public String DirectoryId
        {
            get { return directoryId; }
            set { directoryId = value; }
        }
        public List<FileResponse> Files
        {
            get { return files; }
            set { files = value; }
        }
        public Collection<DirectoryResponse> SubDirectories
        {
            get { return subDirectories; }
            set { subDirectories = value; }
        }

    }
}
