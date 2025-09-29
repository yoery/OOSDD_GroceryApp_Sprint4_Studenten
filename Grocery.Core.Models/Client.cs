namespace Grocery.Core.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string PasswordHash { get; set; }
        public Role Role { get; set; } = Role.None;

        public Client(int id, string name, string emailAddress, string passwordHash, Role role = Role.None)
        {
            Id = id;
            Name = name;
            EmailAddress = emailAddress;
            PasswordHash = passwordHash;
            Role = role;
        }
    }
}