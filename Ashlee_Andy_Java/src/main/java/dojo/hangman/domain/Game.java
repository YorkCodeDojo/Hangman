package dojo.hangman.domain;

import java.util.List;
import java.util.function.Predicate;
import java.util.regex.Pattern;
import java.util.stream.Collectors;

public class Game {
    private String id;
    private List<String> lettersGuessed;
    private List<String> progress;
    private boolean complete;
    private boolean won;

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public List<String> getLettersGuessed() {
        return lettersGuessed;
    }

    public void setLettersGuessed(List<String> lettersGuessed) {
        this.lettersGuessed = lettersGuessed;
    }

    public List<String> getProgress() {
        return progress;
    }

    public void setProgress(List<String> progress) {
        this.progress = progress;
    }

    public boolean isComplete() {
        return complete;
    }

    public void setComplete(boolean complete) {
        this.complete = complete;
    }

    public boolean isWon() {
        return won;
    }

    public void setWon(boolean won) {
        this.won = won;
    }

    public String getProgressAsWord(){
        return progress.stream().map( c-> (c != null) ? c : "_").collect(Collectors.joining());
    }

}
