namespace HangmanTest.Services.DTOs
{
    public class Game
    {
        public string id { get; set; }

        public string[] lettersGuessed { get; set; }

        public string[] progress { get; set; }

        public int misses { get; set; }

        public bool complete { get; set; }

    }
}

