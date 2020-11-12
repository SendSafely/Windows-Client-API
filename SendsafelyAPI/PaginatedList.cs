using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SendSafely
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PaginatedList<T> : List<T>
    {
        private int _rowIndex = 0;
        private int _rowsReturned;
        private int _nextRowIndex;
        private bool _rowsCapped;

        [JsonProperty(PropertyName = "rowIndex")]
        public int RowIndex
        {
            get { return _rowIndex; }
            set { _rowIndex = value; }
        }

        [JsonProperty(PropertyName = "rowsReturned")]
        public int RowsReturned
        {
            get { return _rowsReturned; }
            set { _rowsReturned = value; }
        }

        [JsonProperty(PropertyName = "nextRowIndex")]
        public int NextRowIndex
        {
            get { return _nextRowIndex; }
            set { _nextRowIndex = value; }
        }

        [JsonProperty(PropertyName = "rowsCapped")]
        public bool RowsCapped
        {
            get { return _rowsCapped; }
            set { _rowsCapped = value; }
        }
    }

    
}
