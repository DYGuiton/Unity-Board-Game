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
    public MapController mapController;

    [SerializeField]
    public GameParameters gameParameters;

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
        gameParameters = new GameParameters(1, 1);

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
        playerManager.noTurnsLeft = false;
        playerManager.NextTurn();

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

        //Create the Players
        RegisterPlayers();

        //Begin the Game by setting the phase to Player Phase
        StartCoroutine(GameLoop());
    }

    private void ConnectGameManagerObjects() {
        cameraManager.cameraHolder = GameObject.Find("Cameras");
        mapController = GameObject.Find("Map").GetComponent<MapController>();
        playerManager = GameObject.Find("Players").GetComponent<PlayerManager>();
        userInterfaceManager = GameObject.Find("UserInterface").GetComponent<UserInterfaceManager>();
    }

    private void InitializeMap() {
        mapController.setMapTransform();
        mapController.GenerateFlatHexagonBlueprint(gameParameters.mapSize);
        mapController.GenerateFlatHexField();
    }

    private void RegisterPlayers() {
        for (int i = 0; i < gameParameters.playerCount; i++) {
            if (i < 4) {
                registerPlayer(mapController.townTileList[i]);
            }
        }
    }

    private void registerPlayer(Tile townTile) {
        GameObject newPlayer = playerManager.CreatePlayer(townTile);
        cameraManager.SetupPlayerCamera(newPlayer);
        userInterfaceManager.SetupPlayerUI(newPlayer);
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
}
