
using ModelAdvertisement;
using ModelLookup;
using ModelUser;
using System.Collections.Generic;

namespace ServiceUsers
{
    public interface IUserService
    {
        bool AddUser(Users user);
        bool UpdatePassword(Users user);
        IList<Users> UpdateUser(List<Users> tasks);
        IList<Users> DeleteUser(List<Users> tasks);
        List<Users> GetAll(string UserName);
        Users GetUsersDetail(int Id);
        Users GetUsersDetail(string MobileNumber);
        string IsValidUserAndPasswordCombination(string userRef, string Password);
        List<UserType> GetUserType();
        List<Advertisement> GetAdvertisement();
        Users GetUserInfo(string userRef, string Password);
        string ValidateEmailandMobile(string Email, string Mobile);

        #region User Permission
        bool AddUserPermission(User_SubMenuDTO user);
        List<UserPermissionDTO> GetUserPermissionforUser(int UserId);
        #endregion
    }
}
