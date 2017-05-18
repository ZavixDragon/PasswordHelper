using PasswordHelper;

namespace PasswordHelperWebApi.PasswordHandling
{
    public interface IPasswordGenerationFactory
    {
        PasswordGeneration Create(GeneratePasswordRequest request);
    }
}
