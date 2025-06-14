using CoffeHub.Models;

namespace CoffeHub.Repo
{
    public interface IValidation
    {
        bool IsValidMobileNumber(long number);
        bool IsSamePasswordAgain(string password1, string password2);
        bool IsValidPincode(long pin);

        bool IsUserExists(string username, string password);
    }
    public class ValidateUser : IValidation
    {
        private readonly AppDbContext _appContext;
        private static List<UserDetails> _userdetailsList = new List<UserDetails>();

        public ValidateUser(AppDbContext appDbContext)
        {
            this._appContext = appDbContext;
            _userdetailsList = _appContext.UserDetails.ToList();
        }

        public bool IsValidMobileNumber(long number)
        {
            string numbers = number.ToString();
            if (numbers.Length == 10)
                return true;
            else
                return false;
        }
        public bool IsSamePasswordAgain(string password1, string password2)
        {
            if (password1 == password2)
            {
                return true;
            }
            return false;
        }
        public bool IsValidPincode(long pin)
        {
            string pinasString = pin.ToString();
            if (pinasString.Length != 6)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool IsUserExists(string username, string password)
        {
            if(_userdetailsList.Exists(x=>x.Username == username && x.PasswordAgain == password))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


    }
}
