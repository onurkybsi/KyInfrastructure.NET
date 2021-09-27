namespace KybInfrastructure.Demo.Client
{

    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
    }

    public enum UserRole
    {
        User = 0,
        Admin
    }

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
