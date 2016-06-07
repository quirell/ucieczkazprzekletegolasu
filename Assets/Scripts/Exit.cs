using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour {


    void OnTriggerEnter(Collider other) {
        if (other.transform.root.gameObject.tag == "player") {
            
            SceneManager.LoadScene("ending");
        }

    }

}
