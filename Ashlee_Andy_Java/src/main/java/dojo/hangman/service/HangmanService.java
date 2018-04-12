package dojo.hangman.service;

import dojo.hangman.domain.Account;
import dojo.hangman.domain.Auth;
import dojo.hangman.domain.Game;
import dojo.hangman.domain.Letter;
import org.springframework.http.*;
import org.springframework.http.client.OkHttp3ClientHttpRequestFactory;
import org.springframework.stereotype.Service;
import org.springframework.web.client.RestTemplate;

import java.util.Collections;

@Service
public class HangmanService {

    private final String ROOT_URL = "https://dojo-hangman-server.herokuapp.com/api/";
    private OkHttp3ClientHttpRequestFactory requestFactory = new OkHttp3ClientHttpRequestFactory();
    private final RestTemplate restTemplate = new RestTemplate(requestFactory);

    private HttpHeaders createHeaders(Auth token) {
        HttpHeaders headers = new HttpHeaders();
        headers.setAccept(Collections.singletonList(MediaType.APPLICATION_JSON));
        headers.setContentType(MediaType.APPLICATION_JSON);
        headers.set("x-access-token", token.getToken());
        return headers;
    }

    public Auth authenticate(String username, String password){
        Account account = new Account(username, password);
        ResponseEntity<Auth> response = restTemplate.postForEntity(ROOT_URL + "auth/login", account, Auth.class);
        return response.getBody();
    }

    public Game createGame(Auth token){
        HttpEntity<String> requestEntity = new HttpEntity<>("", createHeaders(token));
        ResponseEntity<Game> response = restTemplate.exchange(ROOT_URL + "games", HttpMethod.POST, requestEntity, Game.class);
        return response.getBody();
    }

    public Game guessLetter(Auth token, String letter){
        HttpHeaders headers = createHeaders(token);
        HttpEntity<Letter> requestEntity = new HttpEntity<>(new Letter(letter), headers);
        ResponseEntity<Game> response = restTemplate.exchange(ROOT_URL + "games/current", HttpMethod.PATCH, requestEntity, Game.class);
        return response.getBody();
    }

}
