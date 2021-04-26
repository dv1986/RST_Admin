using System;
using System.Collections.Generic;
using System.Text;

namespace ModelUser
{
    public class UserPermissionDTO
    {
        public int MenuId { get; set; }
        public string MenuName { get; set; }
        public string IconName { get; set; }

        public bool IsChecked { get; set; }

        public List<UserPermission> Children { get; set; } = new List<UserPermission>();
    }
}
