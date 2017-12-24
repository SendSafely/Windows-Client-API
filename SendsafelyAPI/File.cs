using System;
using System.Collections.Generic;
using System.Text;

namespace SendSafely
{
    /// <summary>
    /// A class describing a sendsafely file.
    /// </summary>
    public class File
    {
        private String fileName;
        private String fileId;
        private long fileSize;
        private int parts;

        public File()
        {
        }

        public File(string fileId, string fileName, long fileSize, int parts)
        {
            this.fileId = fileId;
            this.fileName = fileName;
            this.fileSize = fileSize;
            this.parts = parts;
        }

        /// <summary>
        /// The file name of the given file.
        /// </summary>
        public String FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        /// <summary>
        /// The file ID of the given file.
        /// </summary>
        public String FileId
        {
            get { return fileId; }
            set { fileId = value; }
        }

        /// <summary>
        /// The file size of the given file.
        /// </summary>
        public long FileSize
        {
            get { return fileSize; }
            set { fileSize = value; }
        }

        /// <summary>
        /// The number of parts this file is divided into.
        /// </summary>
        public int Parts
        {
            get { return parts; }
            set { parts = value; }
        }
    }
}
