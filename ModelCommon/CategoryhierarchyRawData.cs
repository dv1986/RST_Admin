using System;
using System.Collections.Generic;
using System.Text;

namespace ModelCommon
{
    public class CategoryhierarchyRawData
    {
        public int RowId { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }
        public int Level { get; set; }
    }
}
