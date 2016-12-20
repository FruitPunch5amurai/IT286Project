using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour {
    //public GameObject arrow;
    public GameObject[] WeaponChoices = new GameObject[3];
    public static GameObject StarterWeapon;

    public Object SceneToLoad;

    int CurrentOption = 0;

	// Use this for initialization
	void Start () {
        //arrow.transform.position = transform.GetChild(0).position;
	}
	
	// Update is called once per frame
	void Update () {
        /*
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            CurrentOption += 1;
            if (CurrentOption >= transform.childCount) {
                CurrentOption = 0;
            }
            UpdateOption();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            CurrentOption -= 1;
            if (CurrentOption < 0) {
                CurrentOption = transform.childCount - 1;
            }
            UpdateOption();
        }
        */

        if (Input.GetButtonDown("BasicAttack")) {
            //Debug.Log("Starting the level with weapon: " + CurrentOption);
            //Set the selected weapon....
            //StarterWeapon = WeaponChoices[CurrentOption];

            //Replace this with the final "Level 1" or its equivalent
            SceneManager.LoadScene(SceneToLoad.name);
        }
	}
    /*
    void UpdateOption() {
        arrow.transform.position = transform.GetChild(CurrentOption).position;
    }
    */
}
