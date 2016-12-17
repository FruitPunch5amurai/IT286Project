using UnityEngine;
using System.Collections;

public class EnemyItemDrop : MonoBehaviour
{

    public GameObject[] PossibleItemDrops;
    public float ItemDropRatePercent;


    // Use this for initialization
    void Start()
    {

    }
    public void DropItem()
    {
        int indexLenth = PossibleItemDrops.Length;
        if (PossibleItemDrops.Length == 0)
            return;
        float randomNum = Random.Range(0, 100);
        if(randomNum < ItemDropRatePercent)
        {
            //Drop random Item
            int randomIndex = Random.Range(0, indexLenth);
            Instantiate(PossibleItemDrops[randomIndex],transform.position,Quaternion.identity);
        }
    }
}
