using UnityEngine;
using System.Collections;


[System.Serializable]
public class Boundary
{
    public float xMin, xMax, yMin, yMax;
}

public class CameraMove : MonoBehaviour {

    public Boundary CameraBoundary;
    public GameObject Focus;
    public float SmoothSpeed;
    public float Distance;

    private SpriteRenderer spriteBounds;
    private Vector2 Velocity;
    // Use this for initialization
    void Start ()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Focus != null)
        {
            float posX = Mathf.SmoothDamp(transform.position.x, Focus.transform.position.x, ref Velocity.x, SmoothSpeed);
            float posY = Mathf.SmoothDamp(transform.position.y, Focus.transform.position.y, ref Velocity.y, SmoothSpeed);


            Vector3 pos =  new Vector3(posX, posY, Focus.transform.position.z - Distance);


            pos = new Vector3
                  (Mathf.Clamp(pos.x, CameraBoundary.xMin, CameraBoundary.xMax),
                  Mathf.Clamp(pos.y, CameraBoundary.yMin, CameraBoundary.yMax),
                  transform.position.z
                  );

          transform.position = Vector3.Lerp(transform.position,pos,.2f);
        }
    }
    public void SetCameraBoundary()
    {
        if (Focus != null)
        {
            GameObject Room = Focus.GetComponent<PlayerControl>().CurrentRoom;
            Vector3 Size = Room.GetComponent<BoxCollider>().size;

            float vertExtent = GetComponent<Camera>().orthographicSize;
            float horzExtent = vertExtent * Screen.width / Screen.height;
            CameraBoundary.xMin = Room.transform.position.x + (float)(horzExtent - Size.x / 2.0f);
            CameraBoundary.xMax = Room.transform.position.x + (float)(Size.x / 2.0f - horzExtent);
            CameraBoundary.yMin = Room.transform.position.y + (float)(vertExtent - Size.y / 2.0f);
            CameraBoundary.yMax = Room.transform.position.y + (float)(Size.y / 2.0f - vertExtent);
            StartCoroutine(PauseWaitForSeconds());
            Debug.Log("CameraSet");
        }
    }
    IEnumerator PauseWaitForSeconds()
    {
        GameManager.singleton.CurrentGameState = GameManager.GameState.Pause;
        yield return new WaitForSeconds(.5f);
        GameManager.singleton.CurrentGameState = GameManager.GameState.Play;

    }
}
