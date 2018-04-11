using System.Collections.Generic;

namespace HangmanTest.Services.DTOs
{
    class Dictionary
    {
        public List<Result> results { get; set; }
    }

    class Result
    {
        public List<LexicalEntry> lexicalEntries { get; set; }

    }

    class LexicalEntry
    {
        public List<Entry> entries { get; set; }
    }

    class Entry
    {
        public List<Sense> senses { get; set; }

    }

    class Sense
    {
        public List<string> definitions { get; set; }

    }
}
