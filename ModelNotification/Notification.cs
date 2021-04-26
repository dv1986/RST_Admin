using System;

namespace ModelNotification
{
	public class Notification
	{
		public int RowId { get; set; }
		public int ProductId { get; set; }
		public string TextPrompt { get; set; }
		public bool IsActive { get; set; }
		public string ProductTitle { get; set; }
		public string SKUCode { get; set; }
		public string ShortDescription { get; set; }
		public string ModelNumber { get; set; }
		public int ModelYear { get; set; }
        public string Message { get; set; }
    }
}
