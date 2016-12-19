using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour {


    static GameManager instance = null;

    public GameObject dungeonGenerator;
    public GameObject m_RootRoom;
    public GameObject Dungeon;
    public GameObject Player;
    public GameObject AStarGrid;
    public GameObject BulletManager;
    public GameObject Canvas;
    public List<GameObject> CurrentRoomEnemies;
    public List<GameObject> PathRequests;

    public Image Key;
    private Vector3 SpawnPosition;


    public bool HasBossKey;


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
        QualitySettings.vSyncCount =2;
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        StartGame();
    }

    void Start ()
    {
        HasBossKey = false;
        Key.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            //RemoveEnemiesFromRoom(Player.GetComponent<PlayerControl>().CurrentRoom);
            PlayerDeathRestart();
        }
    }

    void GenerateLevel()
    {


    }
    void PlayerDeathRestart()
    {
        //PlayerControl pm = Player.GetComponent<PlayerControl>();
        //RemoveEnemiesFromRoom(pm.CurrentRoom);
        //SpawnPlayer(new Vector2(Dungeon.transform.GetChild(0).gameObject.transform.GetChild(0).position.x,
        //    Dungeon.transform.GetChild(0).gameObject.transform.GetChild(0).position.y));
        //pm.CurrentRoom = Dungeon.transform.GetChild(0).gameObject;
        //SpawnEnemies(Player.GetComponent<PlayerControl>().CurrentRoom);
        //Camera.main.GetComponent<CameraMove>().SetCameraBoundary();
        //AdjustAStarGridToRoom(Dungeon.transform.GetChild(0).gameObject);
        GetComponent<FadeInOut>().EndScene(0);
    }
    void SpawnPlayer(Vector2 destination)
    {
        if (Player == null)
        {
           Player =  Instantiate((GameObject)Resources.Load("Prefabs/Player", typeof(GameObject)));
            Player.GetComponent<PlayerControl>().CurrentRoom = m_RootRoom;
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
        Dungeon.transform.position = new Vector3(Dungeon.transform.position.x, Dungeon.transform.position.y,.001f);
        Dungeon.layer = 10;
        m_RootRoom = Dungeon.transform.GetChild(0).gameObject;
        SpawnPlayer(new Vector2(m_RootRoom.transform.GetChild(0).position.x, m_RootRoom.transform.GetChild(0).position.y));
        if (m_RootRoom.GetComponent<Enemies>().GenerateRandomAmount == true)
            SpawnEnemiesRandom(m_RootRoom);
        else
            SpawnEnemies(m_RootRoom);
        Camera.main.GetComponent<CameraMove>().Focus = Player;
        Camera.main.GetComponent<CameraMove>().SetCameraBoundary();
        AdjustAStarGridToRoom(m_RootRoom);
    }

    /*
     * Handles transitioning rooms for player
     */
    public void TransitionRoom(GameObject Room, Vector3 NewPosition)
    {
        Debug.Log("RoomTransition");
        PlayerControl pm = Player.GetComponent<PlayerControl>();
        RemoveEnemiesFromRoom(pm.CurrentRoom);
        pm.CurrentRoom = Room;
        SpawnPosition = NewPosition;
        Player.transform.position = new Vector3(NewPosition.x, NewPosition.y, -.001f);
        Camera.main.GetComponent<CameraMove>().SetCameraBoundary();
        AdjustAStarGridToRoom(Room);
        if (Room.GetComponent<Enemies>().GenerateRandomAmount == true)
            SpawnEnemiesRandom(Room);
        else
            SpawnEnemies(Room);
        BulletManager.GetComponent<BulletManager>().ClearBullets();
    }

    /*
     * Changes the AStarPathfindingGrids position and dimensions to match room
     */
    public void AdjustAStarGridToRoom(GameObject Room)
    {
        AStarGrid.GetComponent<AstarPath>().astarData.gridGraph.center = Room.transform.position;
        AstarPath.active.Scan();
    }

    /*
     * Clears enemies from the room
     */
    public void RemoveEnemiesFromRoom(GameObject Room)
    {
        foreach (GameObject enemy in CurrentRoomEnemies)
        {
            if (enemy != null)
            {
                if (enemy.GetComponent<EnemyAI>().path != null)
                {
                    enemy.GetComponent<EnemyAI>().path.Release(enemy);
                }
                Destroy(enemy);
            }
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
                enemy = (GameObject)Instantiate(enemies.ListOfEnemies[i], new Vector3(Room.transform.position.x, Room.transform.position.y, -.001f), rot);
                enemy.GetComponent<EnemyAI>().CurrentRoom = Player.GetComponent<PlayerControl>().CurrentRoom;
                CurrentRoomEnemies.Add(enemy);
            }
        }
    }
    public void SpawnEnemiesRandom(GameObject Room)
    {
        Enemies enemies = Room.GetComponent<Enemies>();
        GameObject enemy;
        if (enemies.ListOfEnemies.Count == 0)
            return;
        int Amount = Random.Range(3, 6);
        for (int i = 0; i < Amount; i++)
        {
            int index = Random.Range(0, enemies.ListOfEnemies.Count);
            enemy = (GameObject)Instantiate(enemies.ListOfEnemies[index], new Vector3(Room.transform.position.x, Room.transform.position.y, -.001f), Quaternion.identity);
            enemy.GetComponent<EnemyAI>().CurrentRoom = Player.GetComponent<PlayerControl>().CurrentRoom;
            CurrentRoomEnemies.Add(enemy);
        }
    }

    public IEnumerator ProceedToNextStage()
    {
        yield return new WaitForSeconds(2f);
        PlayerDeathRestart();
    }
    IEnumerator ProcessPathRequests()
    {
        if (PathRequests.Count > 0)
        {
            EnemyAI a = PathRequests[0].GetComponent<EnemyAI>();
            Seeker s = PathRequests[0].GetComponent<Seeker>();
            s.StartPath(a.transform.position, a.Target, a.OnPathComplete);
            PathRequests.RemoveAt(0);
        }
        yield return new WaitForSeconds(1.0f / 10);
        //StartCoroutine(ProcessPathRequests());
    }

    /*
     *Adds a Boss key 
     */
     public void GiveBossKey(bool k)
    {
        Key.enabled = k;
        HasBossKey = k;
    }
}

