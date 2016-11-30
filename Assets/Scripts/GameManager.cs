using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    static GameManager instance = null;

    public GameObject dungeonGenerator;
    public GameObject m_RootRoom;
    public GameObject Dungeon;
    public GameObject Player;
    public GameObject AStarGrid;
    public List<GameObject> CurrentRoomEnemies;


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
        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            RemoveEnemiesFromRoom(Player.GetComponent<PlayerMove>().CurrentRoom);
        }
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
        Debug.Log( Dungeon.GetComponent<DunGen.Dungeon>().Bounds.size.x);
        m_RootRoom = Dungeon.transform.GetChild(0).gameObject;
        SpawnPlayer(new Vector2(m_RootRoom.transform.GetChild(0).position.x, m_RootRoom.transform.GetChild(1).position.y));
        SpawnEnemies(Player.GetComponent<PlayerMove>().CurrentRoom);
        
        //for (int i = 0; i < 10; i++)
        //{ 
        //    GameObject enemy = Instantiate((GameObject)Resources.Load("Prefabs/Octorok", typeof(GameObject)));
        //    enemy.transform.position = new Vector3(0, 0, -.001f);
        //    enemy.GetComponent<EnemyAI>().CurrentRoom = Dungeon.transform.GetChild(0).gameObject;
        //} 
        Camera.main.GetComponent<CameraMove>().Focus = GameManager.singleton.Player;
        Camera.main.GetComponent<CameraMove>().SetCameraBoundary();
        //AdjustAStarGridToRoom();
    }

    /*
     * Handles transitioning rooms for player
     */
    public void TransitionRoom(GameObject Room, Vector3 NewPosition)
    {
        PlayerMove pm = Player.GetComponent<PlayerMove>();
        Debug.Log("Trigger: RoomTransition");
        RemoveEnemiesFromRoom(pm.CurrentRoom);
        pm.CurrentRoom = Room;
        Player.transform.position = new Vector3(NewPosition.x, NewPosition.y, -.001f);
        Camera.main.GetComponent<CameraMove>().SetCameraBoundary();
    }

    /*
     * 
     */
    public void AdjustAStarGridToRoom(GameObject Room)
    {

    }

    /*
     * Clears enemies from the room
     */
    public void RemoveEnemiesFromRoom(GameObject Room)
    {
        foreach(GameObject enemy in CurrentRoomEnemies)
        {
            enemy.GetComponent<EnemyAI>().path.Release(enemy);
            Destroy(enemy);
        }
        CurrentRoomEnemies.Clear();
    }
    /*
     * Handles Spawning of enemies in the room
     */
    public void SpawnEnemies(GameObject Room)
    {
        Enemies enemies = Room.GetComponent<Enemies>();
        Quaternion rot = Quaternion.identity;
        GameObject enemy;

        for(int i = 0; i < enemies.ListOfEnemies.Count;i++)
        {
            int amount = enemies.AmountOfEnemies[i];
            for(int j = 0; j < amount;j++)
            {
                enemy = (GameObject)Instantiate(enemies.ListOfEnemies[i], new Vector3(0,0,-.001f), rot);
                enemy.GetComponent<EnemyAI>().CurrentRoom = Player.GetComponent<PlayerMove>().CurrentRoom;
                CurrentRoomEnemies.Add(enemy);
            }
        }
    }
}
