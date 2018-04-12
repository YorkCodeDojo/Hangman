package dojo.hangman;

import dojo.hangman.domain.Auth;
import dojo.hangman.domain.Dictionary;
import dojo.hangman.domain.Game;
import dojo.hangman.service.HangmanService;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.EnableAutoConfiguration;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.context.event.ApplicationReadyEvent;
import org.springframework.context.event.EventListener;

import java.io.IOException;
import java.util.*;
import java.util.function.Function;
import java.util.stream.Collectors;
import java.util.stream.Stream;

@EnableAutoConfiguration
@SpringBootApplication
public class Application {

    Logger log = LoggerFactory.getLogger(Application.class);

    @Autowired
    HangmanService service;

    @EventListener(ApplicationReadyEvent.class)
    public void playGame() throws IOException {
        log.info("Application Started");
        Auth token = service.authenticate("ashlee", "neverguessed");

        for (int i = 1; i < 11; i++) {
            Game game = service.createGame(token);
            log.info("Game Created");
            List<String> words = Dictionary.wordsOfLength(game.getProgress().size());

            while (!game.isComplete()) {
                String filter = createWordFilter(game);
                words = words.stream().filter((s) -> s.matches(filter)).collect(Collectors.toList());
                log.info(String.format("%s : %d Words remaining ", game.getProgressAsWord(), words.size()));
                String bestLetter = pickLetter(game.getLettersGuessed(), words);
                game = service.guessLetter(token, bestLetter);
            }

            if (game.isWon()) {
                log.info("You've Won:" + game.getProgressAsWord());
            } else {
                log.info("You've Lost:"  + game.getProgressAsWord());
            }
        }
    }

    public List<Character> asList(final String string) {
        return new AbstractList<Character>() {
            public int size() {
                return string.length();
            }

            public Character get(int index) {
                return string.charAt(index);
            }
        };
    }


    /**
     * Find the best letter to be used in the next guess.
     *
     * @param lettersGuessed The letters which have been guess already & shouldn't be reused
     * @param words  The words that are still candidate solutions
     * @return The unused letter which appears in the most words
     */
    private String pickLetter(List<String> lettersGuessed, List<String> words) {
        // Create Map which uses the Letter as a Key and the value = count of words containing it
        Map<Character, Long> letterFrequency = words.stream()
                .map((word) -> (new HashSet<>(asList(word)))) // Create a set of the letters in each word
                .flatMap(Collection::stream)              // Concatenate the letter sets into a single stream
                .collect(Collectors.groupingBy(Function.identity(), Collectors.counting() )); // Count occurrences of the letters
        // Remove any letter which have already been guessed
        lettersGuessed.forEach(l -> letterFrequency.remove(l.charAt(0)));
        Character bestLetter = Collections.max(letterFrequency.entrySet(),
                            Comparator.comparingLong(Map.Entry::getValue)).getKey();  // Find the letter with most occurrences
        return bestLetter.toString();
    }

    /**
     * Creates a Regular expression which can be used to filter the words
     *
     * @param game
     * @return
     */
    private String createWordFilter(Game game) {
        final StringBuffer regex = new StringBuffer("^");
        final String missingRegex = getMissingLetterPattern(game.getLettersGuessed());
        game.getProgress().forEach(
                (c) -> {
                    if (c != null) regex.append(c);
                    else regex.append(missingRegex);
                });
        regex.append("$");
        return regex.toString();
    }

    private String getMissingLetterPattern(List<String> lettersGuessed) {
        String excludeLetters = ".";
        if (lettersGuessed.size() > 0) {
            excludeLetters = "[^" + lettersGuessed.stream().collect(Collectors.joining()) + "]";
        }
        return excludeLetters;
    }

    public static void main(String[] args) {
        SpringApplication.run(Application.class, args);
    }

}
