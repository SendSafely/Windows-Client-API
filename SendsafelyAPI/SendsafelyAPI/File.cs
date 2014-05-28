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
    }
}
