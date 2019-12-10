using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestingGameManager : MonoBehaviour {
    //This is a GameManager script that is only used to expedite testing of the MapScene

    public enum Phase {
        StartPhase,
        PlayerPhase,
        EnemyPhase,
        IdlePhase,
        EndPhase
    }

    [SerializeField]
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

        //Game Parameters for the testing game manager is set in the inspector

        //gameParameters = new GameParameters();

        SceneManager.sceneLoaded += OnSceneLoaded;

        NewGame();

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

        ConnectGameManagerObjects();

        InitializeMap();

        SetManagerVariables();

        RegisterPlayers();

        SubscribeToEvents();

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
                registerPlayer(mapController.townTileList[i]);
            }
        }
    }

    private void registerPlayer(Tile townTile) {
        GameObject newPlayer = playerManager.CreatePlayer("testName", townTile);
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
}
