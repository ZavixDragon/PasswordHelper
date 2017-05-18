namespace PasswordHelper
{
    public class Password
    {
        public string Identifier { get; }
        public string Value { get; }

        public Password(string identifier, string value)
        {
            Identifier = identifier;
            Value = value;
        }
    }
}
