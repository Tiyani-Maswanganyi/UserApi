using System.Xml.Serialization;

namespace UserApi.Models
{
    [Serializable]
    [XmlRoot("Users")]
    public class UserList
    {
        [XmlElement("User")]
        public List<User> Users { get; set; } = new List<User>();
    }
}
