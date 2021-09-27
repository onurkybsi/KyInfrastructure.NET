namespace KybInfrastructure.Demo.Business
{
    public class User
    {
        public string Id { get; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public string Token { get; set; }
        public UserRole Role { get; set; }
    }
}
