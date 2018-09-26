using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkovText
{
    class MarkovModel
    {
        CharacterFunction[] firstCharacters;
        CharacterFunction[] middleCharacters;
        CharacterFunction[] finalCharacters;

        int firsts = 0;

        Random rand;

        public MarkovModel()
        {
            rand = new Random();

            firstCharacters = InitCharacterFunctions();
            middleCharacters = InitCharacterFunctions();
            finalCharacters = InitCharacterFunctions();
        }

        public string GenerateWord(int minLen, int maxLen)
        {
            string ret = "";
            int prev = 0;

            var wordLength = rand.Next(minLen, maxLen + 1);

            ret += FirstChar(firstCharacters, ref prev);
            ret += NextChar(firstCharacters, ref prev);

            for (int i = 0; i < wordLength - 3; ++i)
            {
                ret += NextChar(middleCharacters, ref prev);
            }

            ret += NextChar(finalCharacters, ref prev);

            return ret;
        }

        private char FirstChar(CharacterFunction[] arr, ref int prev)
        {
            var firstCharCumulative = 1 + rand.Next(firsts - 1);


            int index = 0;
            int cumulative = 0;

            do
            {
                cumulative += arr[index].occurrences;
                index++;
            } while (cumulative < firstCharCumulative && index < 25);

            index--;
            prev = index;

            return arr[index].current;
        }

        private char NextChar(CharacterFunction[] arr, ref int prev)
        {
            var nextCharCumulative = 1 + rand.Next(arr[prev].occurrences - 1);

            int index = 0;
            int cumulative = 0;

            do
            {
                cumulative += arr[prev].nextChars[index].occurrences;
                index++;
            } while (cumulative < nextCharCumulative && index < 25);

            index--;

            char ret = arr[prev].nextChars[index].character;

            prev = index;

            return ret;
        }

        public void AddWords(string[] words)
        {
            foreach (var word in words)
            {
                this.AddWord(word);
            }
        }

        public void AddWord(string word)
        {
            if (word.Length >= 2)
            {
                var lWord = word.ToLower();

                AddFirstCharacter(lWord);
                AddMiddleCharacters(lWord);
                AddEndCharacter(lWord);
            }
        }

        private void AddFirstCharacter(string word)
        {
            firsts++;

            char curr = word[0];
            char next = word[1];

            AddCharacter(firstCharacters, curr, next);
        }

        private void AddMiddleCharacters(string word)
        {
            for (int i = 1; i < word.Length - 2; ++i)
            {
                char curr = word[i];
                char next = word[i + 1];

                AddCharacter(middleCharacters, curr, next);
            }
        }

        private void AddEndCharacter(string word)
        {
            var lastIndex = word.Length - 1;

            char curr = word[lastIndex - 1];
            char next = word[lastIndex];

            AddCharacter(finalCharacters, curr, next);
        }

        private void AddCharacter(CharacterFunction[] arr, char curr, char next)
        {
            if (curr >= 'a' && curr <= 'z' && next >= 'a' && next <= 'z')
            {
                arr[curr - 'a'].nextChars[next - 'a'].occurrences += 1;

                arr[curr - 'a'].totalNexts += 1;
                arr[curr - 'a'].occurrences += 1;
            }
            else
            {
                throw new ArgumentException("Non-lowercase character in AddEndCharacter");
            }
        }

        struct CharInstance
        {
            public char character;

            public int occurrences;
        }

        struct CharacterFunction
        {
            public CharacterFunction(char current)
            {
                this.current = current;
                this.occurrences = 0;
                this.totalNexts = 0;

                nextChars = new CharInstance[26];

                for (char c = 'a'; c <= 'z'; ++c)
                {
                    nextChars[c - 'a'] = new CharInstance() { character = c, occurrences = 0 };
                }
            }

            public char current;
            public int occurrences;

            public int totalNexts;
            public CharInstance[] nextChars;
        }

        private CharacterFunction[] InitCharacterFunctions()
        {
            var characterFunctions = new CharacterFunction[26];

            for (char c = 'a'; c <= 'z'; ++c)
            {
                characterFunctions[c - 'a'] = new CharacterFunction(c);
            }

            return characterFunctions;
        }
    }
}