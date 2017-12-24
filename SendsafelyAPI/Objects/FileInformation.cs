using System;
using System.Collections.Generic;
using System.Text;

namespace SendSafely.Objects
{
    public class FileInformation
    {
        private String fileId;
        private String fileName;
        private String fileSize;
        private String createdByEmail;
        private String createdById;
        private DateTime uploaded;
        private String uploadedStr;
        private List<FileInformation> oldVersions;
        private int fileVersion;
        private int fileParts;

        public String FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        public String FileId
        {
            get { return fileId; }
            set { fileId = value; }
        }

        public String FileSize
        {
            get { return fileSize; }
            set { fileSize = value; }
        }
        public String CreatedByEmail
        {
            get { return createdByEmail; }
            set { createdByEmail = value; }
        }
        public String CreatedById
        {
            get { return createdById; }
            set { createdById = value; }
        }
        public DateTime Uploaded
        {
            get { return uploaded; }
            set { uploaded = value; }
        }
        public String UploadedStr
        {
            get { return uploadedStr; }
            set { uploadedStr = value; }
        }
        public List<FileInformation> OldVersions
        {
            get { return oldVersions; }
            set { oldVersions = value; }
        }
        public int FileVersion
        {
            get { return fileVersion; }
            set { fileVersion = value; }
        }
        public int FileParts
        {
            get { return fileParts; }
            set { fileParts = value; }
        }
    }
}
