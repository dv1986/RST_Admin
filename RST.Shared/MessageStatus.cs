using System;
using System.Collections.Generic;
using System.Text;

namespace RST.Shared
{
    public static class MessageStatus
    {
        public static string Success
        {
            get
            {
                return "Data Retreived Successfully !";
            }
            set { }
        }
        public static string Error
        {

            get
            {
                return "An Error Occurred !";
            }
            set { }
        }
        public static string Create
        {
            get
            {
                return "Record Created Successfully !";
            }
            set { }
        }
        public static string Update
        {
            get
            {
                return "Data Update Successfully !";
            }
            set { }
        }
        public static string Delete
        {
            get
            {
                return "Data Deleted Successfully !";
            }
            set { }
        }
    }
}
