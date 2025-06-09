using CoffeHub.Models;
using CoffeHub.Repo;

namespace CoffeHub.Service
{
    public interface ICreateAccount
    {
        void AddUser(UserDetails user);
    }


    public class CreateUserAccount : ICreateAccount
    {
        private readonly AppDbContext _appDbContext ;

        public CreateUserAccount(AppDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
        }

        public void AddUser(UserDetails user)
        {            
            _appDbContext.UserDetails.AddRange(user);
             _appDbContext.SaveChanges();
        }
    }
}
