using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private GameManager GameManage;
    private bool loading = false;

    private void OnTriggerStay(Collider other) {
        if(!loading && other.gameObject.tag == "User"){

            //We should have it to where we tell the player to start fading out and the we wait the fade out duration until we load the next scene
            loading = true;
            GameManage.LoadNextLevel();

        }

    }

}
