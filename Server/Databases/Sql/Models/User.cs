using Server.Databases.Sql.Core;
using System.ComponentModel.DataAnnotations;

namespace Server.Databases.Sql.Models
{
    public class User : IEntity
    {
        [Key]
        [DataType(DataType.EmailAddress)]
        public string Id { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class UserViewModel
    {
        public string Id { get; set; }
        public string Password { get; set; }
    }
}
