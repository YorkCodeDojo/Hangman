using System.Collections.Generic;
using System.Linq;
using HangmanTest.Services.DTOs;

namespace HangmanTest.WordList
{
    class FilterList
    {
        internal IEnumerable<Word> FilterByLength(IEnumerable<Word> words, int wordLength)
        {
            return words.Where(word => word.word.Length == wordLength);
        }

        internal IEnumerable<Word> RemoveWordsWhichDoNotContain(IEnumerable<Word> words, char letter)
        {
            return words.Where(w => !w.word.Contains(letter));
        }

        internal IEnumerable<Word> RemoveWordsWithOutALetterInKnownPosition(IEnumerable<Word> words, int position, char letter)
        {
            return words.Where(w => w.word.Substring(position, 1) == letter.ToString());
        }

        internal char SuggestLetter(IEnumerable<Word> words, List<char> known)
        {
            return words.SelectMany(w => w.word.ToCharArray())
                        .GroupBy(a => a)
                        .OrderByDescending(g => g.Count())
                        .Where(a => !known.Contains(a.Key))
                        .First()
                        .Key;
        }

        internal char SuggestLetterIgnoringDuplicates(IEnumerable<Word> words, List<char> known)
        {
            return words.SelectMany(w => w.word.ToCharArray().Distinct())
                        .GroupBy(a => a)
                        .OrderByDescending(g => g.Count())
                        .Where(a => !known.Contains(a.Key))
                        .First()
                        .Key;
        }

        internal IEnumerable<Word> RemoveWordsNotMatchingMask(IEnumerable<Word> words, string mask, char letter)
        {
            return words.Where(w => MatchesMask(w.word, mask, letter));
        }

        private bool MatchesMask(string word, string mask, char letter)
              => Enumerable.Range(0, word.Length - 1).All(i => ((mask[i] == letter && word[i] == letter) || (mask[i] != letter && word[i] != letter)));
    }
}
