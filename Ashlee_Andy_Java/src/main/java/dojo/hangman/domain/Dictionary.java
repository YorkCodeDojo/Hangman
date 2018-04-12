package dojo.hangman.domain;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.util.List;
import java.util.stream.Collectors;
import java.util.stream.Stream;

public class Dictionary {

    private static List<String> words;

    private static Stream<String> allWords() throws IOException {
        if (words == null) {
            try (InputStream is = Dictionary.class.getClassLoader().getResourceAsStream("dictionary.txt");
                 BufferedReader reader = new BufferedReader(new InputStreamReader(is))) {
                words = reader.lines().collect(Collectors.toList());
            }
        }
        return words.stream();
    }

    public static List<String> wordsOfLength(int size) throws IOException {
        return allWords().filter(w -> w.length() == size).collect(Collectors.toList());
    }
}
