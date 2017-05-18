using PasswordHelper.Common;

namespace PasswordHelper.Characters
{
    public abstract class RandomCharacters : ICharacters
    {
        private char[] _chars;

        public RandomCharacters(string chars) : this(chars.ToCharArray()) {}

        public RandomCharacters(char[] chars)
        {
            _chars = chars;
        }

        public char Next()
        {
            return _chars.Random();
        }
    }
}
