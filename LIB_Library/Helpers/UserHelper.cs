using Core.Helpers;
using LIB.Constants;
using LIB.Entities;
using LIB.UserMng;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LIB.Helpers
{
    public class UserHelper
    {
        public const int MONITORING_INTV = 10000; //ms => 10s
        public static bool CreateNewUser(User newUser, out string errorMessage)
        {
            bool result = false;
            errorMessage = string.Empty;
            if (newUser == null) return result;
            //controllare se esiste la directory degli utenti
            GameFoldersHelper.CheckDirectoryData();
            //controllare se esiste già un nome con questo utente
            string UserFolderPath = $"{Cnst.ApplicationFolderUsers}\\{newUser.Name}";
            if (Directory.Exists(UserFolderPath))
            {
                errorMessage = $"È già presente un utente con il nome {newUser.Name}.";
                return result;
            }

            //creare l'utente
            try
            {
                Directory.CreateDirectory(UserFolderPath);
                //set dates to User
                newUser.Created = newUser.Updated = newUser.LastConnected = DateTime.Now;
                newUser.TotalActiveTime = new TimeSpan(0);
                CheckAndOverrideUserLogInOrder(ref newUser);
                result = WriteUserFile(UserFolderPath, newUser);
            }catch (Exception ex)
            {
                errorMessage = ex.Message;
                return result;
            }

            
            return result;
        }

        public static bool UpdateUser(User user, out string errorMessage)
        {
            bool result = false;
            errorMessage = String.Empty;
            //controllare se esiste la directory degli utenti
            GameFoldersHelper.CheckDirectoryData();
            //controllare se esiste già un nome con questo utente
            string UserFolderPath = $"{Cnst.ApplicationFolderUsers}\\{user.Name}";
            if (!Directory.Exists(UserFolderPath))
            {
                errorMessage = $"Impossibile trovare l'utente {user.Name}";
                return result;
            }

            try
            {
                user.Updated = DateTime.Now;
                CheckAndOverrideUserLogInOrder(ref user);
                result = WriteUserFile(UserFolderPath, user);
            }catch(Exception ex)
            {
                errorMessage = ex.Message;
                return result;
            }

            return result;
        }

        public static bool BaseUpdateUser(User user, out string errorMessage)
        {
            bool result = false;
            errorMessage = String.Empty;
            //controllare se esiste la directory degli utenti
            GameFoldersHelper.CheckDirectoryData();
            //controllare se esiste già un nome con questo utente
            string UserFolderPath = $"{Cnst.ApplicationFolderUsers}\\{user.Name}";
            if (!Directory.Exists(UserFolderPath))
            {
                errorMessage = $"Impossibile trovare l'utente {user.Name}";
                return result;
            }

            try
            {
                user.Updated = DateTime.Now;
                //CheckAndOverrideUserLogInOrder(ref user);
                result = WriteUserFile(UserFolderPath, user);
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return result;
            }

            return result;
        }

        public static bool DeleteUser(string userName, out string errorMessage)
        {
            bool result = false;
            errorMessage = String.Empty;
            string userFolderPath = $"{Cnst.ApplicationFolderUsers}\\{userName}";
            try
            {
                if (Directory.Exists(userFolderPath))
                {
                    Directory.Delete(userFolderPath, true);
                    result = true;
                }
                else
                {
                    errorMessage = $"Impossibile trovare l'utente {userName}.";
                }
            }catch(Exception e)
            {
                errorMessage = e.Message;
            }

            return result;
        }

        public static List<User> GetUsers()
        {
            List<User> users = new List<User>();

            if (GameFoldersHelper.CheckDirectoryData())
            {
                if (Directory.Exists(Cnst.ApplicationFolderUsers))
                {
                    string[] directoryUsers = Directory.GetDirectories(Cnst.ApplicationFolderUsers);
                    User _user;
                    foreach (string directoryUser in directoryUsers)
                    {
                        try
                        {
                            if (!File.Exists($"{directoryUser}\\{Cnst.UserFileName}")) continue;
                            string userXML = File.ReadAllText($"{directoryUser}\\{Cnst.UserFileName}");
                            _user = (User)XmlSerializerBase.DeserializeObjectFromString(userXML, typeof(User));
                            users.Add(_user);
                        }catch (Exception e)
                        {
                            //Log Error
                        }
                    }
                }
            }

            return users;
        }

        public static bool WriteUserFile(string UserFolderPath, User user)
        {
            bool result = false;
            string UserXml = XmlSerializerBase.SerializeObjectToString(user);
            string userFilePath = UserFolderPath + $"\\{Cnst.UserFileName}";
            //if(File.Exists(userFilePath)) File.Delete(userFilePath);
            //File.Create(UserFolderPath + $"_user_{newUser.Name}");
            using (StreamWriter write = File.CreateText(userFilePath))
            {
                write.Write(UserXml);
                result = true;
            }
            return result;
        }
        /// <summary>
        /// Checks if a user with the same login order already exists and overwrites it if necessary
        /// </summary>
        /// <param name="user"></param>
        public static void CheckAndOverrideUserLogInOrder(ref User user)
        {
            if (user == null) return;
            List<User> _users = GetUsers();
            foreach(User currentUser in _users)
            {
                if (currentUser.Name == user.Name) continue; //if the user already exists
                if(currentUser.IsDefaultAccess && currentUser.AutoLoginOrder == user.AutoLoginOrder)
                {
                    if(MessageDialogHelper.ShowConfirmationRequestMessage("È già presente un utente con quest'ordine di accesso. \r\nSovrascriverlo?"))
                    {
                        currentUser.IsDefaultAccess = false;
                        currentUser.AutoLoginOrder = 0;
                        string errorMessage;
                        if(!BaseUpdateUser(currentUser, out errorMessage))
                        {
                            MessageDialogHelper.ShowInfoMessage(errorMessage);
                            return;
                        }
                    }
                    else
                    {
                        user.IsDefaultAccess = false;
                        user.AutoLoginOrder = 0;
                    }
                }
            }
        }
        /// <summary>
        /// log in the pre-set users
        /// </summary>
        public static void AutoLogInUsers()
        {
            List<User> _users = GetUsers();
            if(_users.Count > 0)
            {
                User mainUser = _users.Where(u => u.IsDefaultAccess && u.AutoLoginOrder == 1).FirstOrDefault();
                if (mainUser != null)
                {
                    UserManager.MainLoggedUser.UserLogIn(mainUser);
                }

                User secondUser = _users.Where(u => u.IsDefaultAccess && u.AutoLoginOrder == 2).FirstOrDefault();
                if(secondUser != null)
                {
                    UserManager.SecondLoggedUser.UserLogIn(secondUser);
                }
            }
        }
    }
}
