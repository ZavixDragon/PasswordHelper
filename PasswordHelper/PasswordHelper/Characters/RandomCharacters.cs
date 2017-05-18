using PasswordHelper.Common;

namespace PasswordHelper.CharacterSuppliers
{
    public abstract class RandomCharacterSupplier : ICharacters
    {
        private char[] _chars;

        public RandomCharacterSupplier(string chars) : this(chars.ToCharArray()) {}

        public RandomCharacterSupplier(char[] chars)
        {
            _chars = chars;
        }

        public char Next()
        {
            return _chars.Random();
        }
    }
}
