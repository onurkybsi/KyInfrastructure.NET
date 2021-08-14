namespace KybInfrastructure.Data.Test
{
    public class FakeDatabaseContext : IDatabaseContext
    {
        public virtual void Dispose() { }

        public int SaveChanges()
            => 1;
    }
}
