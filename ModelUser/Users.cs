using System;

namespace ModelUser
{
    public class Users
    {
        public int RowId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string GarageName { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }
        public string ZipCode { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }

        public string Password { get; set; }

        public DateTime? DateOfBirth { get; set; } = null;
        public bool? Status { get; set; }
        public int UserTypeId { get; set; }
        public string UserTypeName { get; set; }

        public int SubscriptionId { get; set; }
        public string SubscriptionName { get; set; }

        public int? AdvertiseImageId { get; set; }
        public string ImageName { get; set; }
        public string ImagePath { get; set; }
        public string ThumbnailPath { get; set; }
        public string Message { get; set; }
    }
}
