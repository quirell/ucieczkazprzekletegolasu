using UnityEngine;
using System.Collections;

public class Chest : MonoBehaviour {

	public PathManager pathManager;

	void OnTriggerEnter(Collider other) {


		//Debug.Log(other.transform.root.gameObject.tag);

		if (other.transform.root.gameObject.tag == "player") {
			GetComponent<ParticleSystem> ().Play ();

            //TODO: add sth to do

            //hide chest
            this.gameObject.SetActive(false);

            pathManager.chestFound = true;
		}



	}


	void OnTriggerExit(Collider other){

		if (other.transform.root.gameObject.tag == "player") {
			GetComponent<ParticleSystem> ().Stop ();

		}

	}
}
