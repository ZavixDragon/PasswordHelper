using PasswordHelper;

namespace PasswordHelperWebApi
{
    public interface IPasswordGenerationFactory
    {
        PasswordGeneration Create(GeneratePasswordRequest request);
    }
}
