using System;

namespace ModelAdvertisement
{
    public class Advertisement
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Url { get; set; }
        public bool IsShowOnHomePage { get; set; }
    }
}
