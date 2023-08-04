using Core.Helpers;
using LIB.Constants;
using LIB.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIB.Helpers
{
    public class UserHelper
    {
        public static bool CreateNewUser(User newUser, out string errorMessage)
        {
            bool result = false;
            errorMessage = string.Empty;
            if (newUser == null) return result;
            //controllare se esiste la directory degli utenti
            GameFoldersHelper.CheckDirectoryData();
            //controllare se esiste già un nome con questo utente
            //TO DO

            //creare l'utente
            try
            {
                string UserFolderPath = $"{Cnst.ApplicationFolderUsers}\\{newUser.Name}";
                Directory.CreateDirectory(UserFolderPath);
                //set dates to User
                newUser.Created = newUser.Updated = newUser.LastConnected = DateTime.Now;
                newUser.TotalActiveTime = new TimeSpan(0);
                string UserXml = XmlSerializerBase.SerializeObjectToString(newUser);
                //File.Create(UserFolderPath + $"_user_{newUser.Name}");
                using(StreamWriter write = File.CreateText(UserFolderPath + "\\_user")) 
                { 
                    write.Write(UserXml);
                    result = true;
                }
            }catch (Exception ex)
            {
                errorMessage = ex.Message;
                return result;
            }

            
            return result;
        }

        public static List<User> GetUsers()
        {
            List<User> users = new List<User>();

            if (GameFoldersHelper.CheckDirectoryData())
            {
                string[] directoryUsers = Directory.GetDirectories(Cnst.ApplicationFolderUsers);
                User _user;
                foreach (string directoryUser in directoryUsers)
                {
                    string userXML = File.ReadAllText($"{directoryUser}\\_user");
                    _user = (User)XmlSerializerBase.DeserializeObjectFromString(userXML, typeof(User));
                    users.Add(_user);
                }
            }

            return users;
        }
    }
}
