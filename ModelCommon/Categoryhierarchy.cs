using System;
using System.Collections.Generic;
using System.Text;

namespace ModelCommon
{
    public class Categoryhierarchy
    {
        public int MenuID { get; set; }
        public int Level { get; set; }
        public string Name { get; set; }
        public List<Categoryhierarchy> _ChildMenus { get; set; }
    }
}
