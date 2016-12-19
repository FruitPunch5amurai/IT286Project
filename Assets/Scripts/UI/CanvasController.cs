using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CanvasController : MonoBehaviour {

    public EnemyHealth BossHealth;
    public GameObject BossHealthBorder;
    public GameObject BossHealthBar;
    public GameObject BossName;

    public void SetBossHealth(GameObject boss)
    {
        BossName.SetActive(true);
        BossName.GetComponent<Text>().text = boss.name;
        BossName.transform.GetChild(0).GetComponent<Text>().text = boss.name;
        BossHealthBorder.gameObject.SetActive(true);

        BossHealth = boss.GetComponent<EnemyHealth>();
        BossHealthBar.transform.localScale = new Vector3(1, BossHealthBar.transform.localScale.y, BossHealthBar.transform.localScale.z);

    }
    public void RemoveBossHealth()
    {
        BossName.SetActive(false);
        BossHealthBorder.SetActive(false);
        BossHealth = null;
    }
    public void UpdateBossHealth()
    {
        float HP = BossHealth.CurrentHealth / BossHealth.MaxHealth;
        Debug.Log(HP);
        BossHealthBar.transform.localScale = new Vector3(HP, BossHealthBar.transform.localScale.y, BossHealthBar.transform.localScale.z);
        if(HP <= 0)
        {
            StartCoroutine(GameManager.singleton.ProceedToNextStage());
        }
    }

    public void UpdatePlayerHealth()
    {

    }

}
