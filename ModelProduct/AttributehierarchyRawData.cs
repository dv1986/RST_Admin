using System;
using System.Collections.Generic;
using System.Text;

namespace ModelProduct
{
    public class AttributehierarchyRawData
    {
        public int RowId { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
        public int Level { get; set; }
    }
}
