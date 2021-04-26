using System;
using System.Collections.Generic;
using System.Text;

namespace ModelProduct
{
    public class Attributehierarchy
    {
        public int ID { get; set; }
        public int Level { get; set; }
        public string Name { get; set; }
        public List<Attributehierarchy> _ChildMenus { get; set; }
    }
}
