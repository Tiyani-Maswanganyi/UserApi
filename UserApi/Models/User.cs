using System.Text.Json.Serialization;

namespace UserApi.Models
{
    [Serializable]
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Cellphone { get; set; }

    }
}
