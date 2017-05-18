using System.Collections.Generic;
using PasswordHelper;
using PasswordHelper.CharacterSuppliers;

namespace PasswordHelperWebApi.PasswordHandling
{
    public class PasswordGenerationFactory : IPasswordGenerationFactory
    {
        public PasswordGeneration Create(GeneratePasswordRequest request)
        {
            var characters = new List<ICharacters>();
            if (request.IncludeLowercaseLetters)
                characters.Add(new LowercaseLetters());
            if (request.IncludeUppercaseLetters)
                characters.Add(new UppercaseLetters());
            if (request.IncludeNumbers)
                characters.Add(new Numbers());
            if (request.IncludeSymbols)
                characters.Add(new Symbols());
            return new PasswordGeneration(request.Length, characters);
        }
    }
}
