using System;
using System.Collections.Generic;
using System.Text;

namespace ModelUser
{
    public class UserPermission
    {
        public int RowId { get; set; }
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public int SubMenuId { get; set; }
        public string SubMenuName { get; set; }

        public string IconName { get; set; }

        public bool isChecked { get; set; }

        public List<UserPermission> Children { get; set; } = new List<UserPermission>();
    }


}
