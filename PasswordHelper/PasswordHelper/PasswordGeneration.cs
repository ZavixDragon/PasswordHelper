using PasswordHelper.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using PasswordHelper.Characters;

namespace PasswordHelper
{
    public class PasswordGeneration
    {
        private readonly int _length;
        private readonly List<ICharacters> _characterSets;
        private readonly string _identifier;
        private readonly Value<Password> _password;

        public PasswordGeneration(string identifier, int length, List<ICharacters> characterSets)
        {
            if (!characterSets.Any())
                throw new ArgumentException("No valid characterList found for generating the password");
            if (length < characterSets.Count)
                throw new ArgumentException("The length was too short to use all inclusions");
            _length = length;
            _characterSets = characterSets;
            _identifier = identifier;
            _password = new Value<Password>(Generate);
        }

        public Password Get()
        {
            return _password.Get();
        }

        private Password Generate()
        {
            var characters = GetOneCharacterFromEachCharacterSet()
                .Concat(GetCharactersFromRandomCharacterSets(_length - _characterSets.Count));
            return new Password(_identifier, string.Concat(Shuffle(characters)));
        }

        private IOrderedEnumerable<char> Shuffle(IEnumerable<char> characters)
        {
            return characters.OrderBy(x => Rng.Int());
        }

        private IEnumerable<char> GetCharactersFromRandomCharacterSets(int count)
        {
            return Enumerable.Range(0, count).Select(x => _characterSets.Random().Next());
        }

        private IEnumerable<char> GetOneCharacterFromEachCharacterSet()
        {
            return _characterSets.Select(x => x.Next());
        }
    }
}
