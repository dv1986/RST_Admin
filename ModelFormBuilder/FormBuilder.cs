using System;
using System.Collections.Generic;
using System.Text;

namespace ModelFormBuilder
{
	public class FormBuilder
	{
		public int RowId { get; set; }
		public string FormName { get; set; }
		public string FormJson { get; set; }
		public bool IsDeleted { get; set; }
        public string Message { get; set; }
    }
}
