namespace CoffeHub.Repo
{
    public interface IValidation
    {
        bool IsValidMobileNumber(long number);
        bool IsSamePasswordAgain(string password1, string password2);
        bool IsValidPincode(long pin);
    }
    public class ValidateUser : IValidation
    {
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
    }
}
