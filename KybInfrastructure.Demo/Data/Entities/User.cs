#nullable disable

namespace KybInfrastructure.Demo.Data
{
    public partial class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public string Token { get; set; }
        public int Role { get; set; }
    }
}
