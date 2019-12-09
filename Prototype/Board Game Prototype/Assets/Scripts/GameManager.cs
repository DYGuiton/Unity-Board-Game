using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour {

    public enum Phase {
        StartPhase,
        PlayerPhase,
        EnemyPhase,
        IdlePhase,
        EndPhase
    }

    public Phase phase;
    public GameParameters gameParameters;
    public MapController mapController;

    public CameraManager cameraManager;
    public PlayerManager playerManager;
    public UserInterfaceManager userInterfaceManager;

    bool victory = false;

    private void Awake() {
        DontDestroyOnLoad(transform.gameObject);
    }

    void Start() {

        //Start the game in a menu system with NoPhase initialized

        //Set Defaults for the mapSize and playerCount
        gameParameters = new GameParameters(1, 1, new string[1]);

        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    private IEnumerator GameLoop() {
        yield return StartCoroutine(RoundStartState());
        yield return StartCoroutine(PlayerState());
        yield return StartCoroutine(EnemyState());
        yield return StartCoroutine(EndState());

        if (victory) {
            Debug.Log("Victory! Exiting Game Now.");
            Application.Quit();
        }
        else {
            StartCoroutine(GameLoop());
        }
    }

    private IEnumerator RoundStartState() {

        Debug.Log("RoundStartState: Enter");
        yield return 10;
        Debug.Log("RoundStartState: Exit");
    }

    private IEnumerator PlayerState() {

        Debug.Log("PlayerState: Enter");
        
        //Set the exit state to false
        playerManager.noTurnsLeft = false;

        //Kickstart the player turns
        playerManager.NextTurn();

        //Stop indefinitely until there are no turns left
        while (playerManager.noTurnsLeft == false) {
            yield return null;
        }
        Debug.Log("PlayerState: Exit");
    }

    private IEnumerator EnemyState() {

        Debug.Log("EnemyState: Enter");
        yield return 10;
        Debug.Log("EnemyState: Exit");
    }

    private IEnumerator IdleState() {
        while (phase == Phase.IdlePhase) {
            yield return null;
        }
    }

    private IEnumerator EndState() {
        Debug.Log("EndState: Enter");
        yield return 10;
        Debug.Log("EndState: Exit");
    }

    public void NewGame() {

        //Connect all the local variables declared at the top to gameobjects in the scene
        ConnectGameManagerObjects();

        //Create the Map using the blueprint system
        InitializeMap();

        //Set Manager Variables
        SetManagerVariables();

        //Create the Players
        RegisterPlayers();

        //Subscribe to events and Delegates
        SubscribeToEvents();

        //Begin the Game by setting the phase to Player Phase
        StartCoroutine(GameLoop());
    }

    private void ConnectGameManagerObjects() {
        cameraManager.cameraHolder = GameObject.Find("Cameras");
        mapController = GameObject.Find("Map").GetComponent<MapController>();
        playerManager = GameObject.Find("Players").GetComponent<PlayerManager>();
        userInterfaceManager = GameObject.Find("UserInterfaceManager").GetComponent<UserInterfaceManager>();
    }

    private void InitializeMap() {
        mapController.setMapTransform();
        mapController.GenerateFlatHexagonBlueprint(gameParameters.mapSize);
        mapController.GenerateFlatHexField();
    }

    private void SetManagerVariables() {
        cameraManager.mapSize = mapController.mapSize;
    }

    private void RegisterPlayers() {
        for (int i = 0; i < gameParameters.playerCount; i++) {
            if (i < 4) {
                registerPlayer(gameParameters.playerNames[i], mapController.townTileList[i]);
            }
        }
    }

    private void registerPlayer(string playerName, Tile townTile) {
        GameObject newPlayer = playerManager.CreatePlayer(playerName, townTile);
        cameraManager.SetupPlayerCamera(newPlayer);
        userInterfaceManager.SetupPlayerUI(newPlayer);
    }

    private void SubscribeToEvents() {
        playerManager.TurnChange += onPlayerTurnChange;
    }

    private void onPlayerTurnChange(String playerName, Material playerMaterial) {

        userInterfaceManager.IndicateChangeTurn(playerName, playerMaterial);

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode aMode) {
        if (SceneManager.GetActiveScene().buildIndex == 1) {
            NewGame();
        }
    }

    public void OnDestroy() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void setMapSize(int mapSize) {
        gameParameters.mapSize = mapSize;
    }

    public void setPlayerCount(int playerCount) {
        gameParameters.playerCount = playerCount;
    }

    public void setPlayerNames(string[] playerNames) {
        gameParameters.playerNames = playerNames;
    }
}
