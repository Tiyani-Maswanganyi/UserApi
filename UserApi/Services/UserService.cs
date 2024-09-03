using System;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;
using UserApi.Models;

namespace UserApi.Services
{
    public class UserService
    {
        public void CreateUser(UserCreateDto user) 
        {
            var users = GetUsers();

            if (user != null)
            {
                var newUser = new User();
                newUser.Name = user.Name;
                newUser.Surname = user.Surname;
                newUser.Cellphone = user.Cellphone;
               
                if (users.Users.Any())
                {
                    newUser.Id = users.Users.Max(u => u.Id) + 1;
                }
                else
                {
                    newUser.Id = 1; 
                }

                users.Users.Add(newUser);
                SaveUsers(users);
            }
        }

        public void SaveUsers(UserList userList)
        {
            string filePath = GetFilePath("user.xml");
            XmlSerializer serializer = new XmlSerializer(typeof(UserList));

            try
            {
                using (FileStream fileStream = File.Open(filePath, FileMode.Truncate))
                {
                    serializer.Serialize(fileStream, userList);
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error saving user data: {ex.Message}");
            }
        }


        public UserList GetUsers()
        {
            string filePath = GetFilePath("user.xml");
            XmlSerializer serializer = new XmlSerializer(typeof(UserList));

            if (File.Exists(filePath) && XMLRootExists("Users"))
            {
                try
                {
                    using (var reader = new StreamReader(filePath))
                    {
                        var list = (UserList)serializer.Deserialize(reader);
                        return list;
                    }

                }
                catch (Exception ex)
                {

                    throw;
                }
            }          
                return new UserList();           
        }

        public void UpdateUser( UserCreateDto updatedUser,int id)
        {
            UserList userList = GetUsers();

 
            User user = userList.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                userList.Users.Remove(user);

                if (user.Name != updatedUser.Name && !string.IsNullOrEmpty(updatedUser.Name))
                {
                    user.Name = updatedUser.Name;
                }

                if(user.Surname != updatedUser.Surname && !string.IsNullOrEmpty(updatedUser.Surname))
                {
                    user.Surname = updatedUser.Surname;
                }

                if (user.Cellphone != updatedUser.Cellphone && !string.IsNullOrEmpty(updatedUser.Cellphone))
                {
                    user.Cellphone = updatedUser.Cellphone;
                }

                userList.Users.Add(user);

                SaveUsers(userList);
            }
        }

        public void DeleteUser(int userId) 
        {
            UserList userList = GetUsers();

            User userToDelete = userList.Users.FirstOrDefault(u => u.Id == userId);

            if (userToDelete != null) 
            {
                userList.Users.Remove(userToDelete);
                SaveUsers(userList);
            }
        }

        private string GetFilePath(string fileName)
        {
            string baseDirectory = AppContext.BaseDirectory;
            return Path.Combine(baseDirectory, fileName);
        }

        private bool XMLRootExists(string expectedRootName)
        {
            try
            {
                string filePath = GetFilePath("user.xml");
                using (XmlReader reader = XmlReader.Create(filePath))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == expectedRootName)
                        {
                            // Check if the root element has at least one child
                            if (reader.Read() && reader.NodeType != XmlNodeType.EndElement)
                            {
                                return true;
                            }
                            else
                            {
                                return false; // Root found but no children
                            }
                        }
                    }
                }
            }
            catch (XmlException)
            {
                return false;
            }
            catch (IOException)
            {
                return false;
            }

            return false;
        }

    }
}
