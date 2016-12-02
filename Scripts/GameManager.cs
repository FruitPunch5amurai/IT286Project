using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    static GameManager instance = null;

    public GameObject dungeonGenerator;
    public GameObject m_RootRoom;
    public GameObject Dungeon;
    public GameObject Player;


    public enum GameState
    {
        TitleScreen,
        Play,
        Pause
    }

    public GameState CurrentGameState;

    private DunGen.RuntimeDungeon m_DungeonGenerator;

    public static GameManager singleton
    {
        get
        {
            return instance;
        }
    }
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        StartGame();
    }

    void Start ()
    {

	}
	
	// Update is called once per frame
	void Update () {
        
    }

    void GenerateLevel()
    {


    }
    void SpawnPlayer(Vector2 destination)
    {
        if (Player == null)
        {
           Player =  Instantiate((GameObject)Resources.Load("Prefabs/Player", typeof(GameObject)));
            Player.GetComponent<PlayerMove>().CurrentRoom = m_RootRoom;
            Player.name = "Player";
        }
        
        Player.transform.position = new Vector3(destination.x, destination.y, -.001f);
    }
    void StartGame()
    {
        CurrentGameState = GameState.Play;
        m_DungeonGenerator = dungeonGenerator.GetComponent<DunGen.RuntimeDungeon>();
        m_DungeonGenerator.Generate();
        Dungeon = GameObject.Find("Dungeon");
        m_RootRoom = Dungeon.transform.GetChild(0).gameObject;
        SpawnPlayer(new Vector2(m_RootRoom.transform.GetChild(0).position.x, m_RootRoom.transform.GetChild(0).position.y));
        Camera.main.GetComponent<CameraMove>().Focus = GameManager.singleton.Player;
        Camera.main.GetComponent<CameraMove>().SetCameraBoundary();


    }
}
