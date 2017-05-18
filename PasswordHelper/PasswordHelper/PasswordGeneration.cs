using PasswordHelper.CharacterSuppliers;
using PasswordHelper.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PasswordHelper
{
    public class PasswordGeneration
    {
        private readonly int _length;
        private readonly List<ICharacters> _characterSets;
        private readonly Value<string> _password;

        public PasswordGeneration(int length, List<ICharacters> characterSets)
        {
            if (!characterSets.Any())
                throw new ArgumentException("No valid characters found for generating the password");
            if (length < characterSets.Count())
                throw new ArgumentException("The length was too short to use all inclusions");
            _length = length;
            _characterSets = characterSets;
            _password = new Value<string>(Generate);
        }

        public string Get()
        {
            return _password.Get();
        }

        private string Generate()
        {
            var characters = _characterSets.Select(x => x.Next()).ToList();
            Enumerable.Range(0, _length - _characterSets.Count()).ForEach(x => characters.Add(_characterSets.Random().Next()));
            return string.Concat(characters.OrderBy(x => Rng.Int()));
        }
    }
}
