namespace KybInfrastructure.Data.Test
{
    public class FakeDatabaseContext : IDatabaseContext
    {
        public bool AreThereAnyChanges()
        {
            throw new System.NotImplementedException();
        }

        public void Rollback()
        {
            throw new System.NotImplementedException();
        }

        public int SaveChanges()
            => 1;

        public virtual void Dispose() { }
    }
}
