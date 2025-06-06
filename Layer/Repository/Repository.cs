

/*
 note: this is only for testing and understanding of generic repository pattern
 not have any benefits for project
 */












namespace CoffeHub.Layer.Repository
{
    public interface IRepository<T>
    {
        Task<T> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        void Post();


    }

    public class Repository<T> where T : class
    {
        
    }

    public interface IRep
    {
        void Get();
        void GetById(int id);
    }
    public class Rep : IRep
    {
        public void Get()
        {
            throw new NotImplementedException();
        }

        public void GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
