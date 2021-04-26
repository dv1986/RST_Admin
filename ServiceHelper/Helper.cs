using ModelUser;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace ServiceHelper
{
    public class Helper
    {
        public static List<UserPermissionDTO> BuildTreeAndReturnRootNodes(List<UserPermission> AllPermission, List<UserPermission> AssignedPermission)
        {
            List<UserPermissionDTO> MenuList = new List<UserPermissionDTO>();
            var byIdLookup = AllPermission.ToLookup(p => new { p.MenuId, p.MenuName, p.IconName });
            foreach (var item in byIdLookup)
            {
                UserPermissionDTO menus = new UserPermissionDTO();
                menus.MenuId = item.Key.MenuId;
                menus.MenuName = item.Key.MenuName;
                menus.IconName = item.Key.IconName;

                foreach (var inneritem in AllPermission)
                {
                    if (inneritem.MenuId == menus.MenuId)
                    {
                        foreach (var p in AssignedPermission)
                        {
                            if (p.SubMenuId == inneritem.SubMenuId)
                            {
                                inneritem.isChecked = true;
                                menus.IsChecked = true;
                            }
                        }
                        menus.Children.Add(inneritem);
                    }
                }
                MenuList.Add(menus);
            }
            return MenuList;
        }

        public static string Base64ToImage(string imgPath)
        {
            if (imgPath == null || imgPath == "")
            {
                return null;
            }
            byte[] imageBytes = System.IO.File.ReadAllBytes(imgPath);
            string base64String = Convert.ToBase64String(imageBytes);
            return "data:image/png;base64," + base64String;
        }

        public static string GetImage(string FileName)
        {
            string responseString;
            var request = (HttpWebRequest)WebRequest.Create("http://content.motoiz.in/api/FileUploader/GetImage?fileName=" + FileName + "");
            request.Method = "GET";
            request.ContentType = "application/json";

            using (var response1 = request.GetResponse())
            {
                using (var reader = new StreamReader(response1.GetResponseStream()))
                {
                    responseString = reader.ReadToEnd();
                }
            }
            if (responseString != null)
            {
                responseString = responseString.Remove(0, 1);
                responseString = responseString.Remove(responseString.Length-1, 1);
            }
            return responseString;
        }

        #region Encryt-Decrypt
        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
        #endregion
    }
}
