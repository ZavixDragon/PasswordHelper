namespace PasswordHelperWebApi.PasswordHandling
{
    public class GeneratePasswordRequest
    {
        public int Length { get; set; } = 16;
        public bool IncludeLowercaseLetters { get; set; } = true;
        public bool IncludeUppercaseLetters { get; set; } = true;
        public bool IncludeNumbers { get; set; } = true;
        public bool IncludeSymbols { get; set; } = false;
    }
}
