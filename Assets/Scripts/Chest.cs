using UnityEngine;
using System.Collections;

public class Chest : MonoBehaviour {

	public PathManager pathManager;

	void OnTriggerEnter(Collider other) {


		Debug.Log(other.transform.root.gameObject.tag);

		if (other.transform.root.gameObject.tag == "player") {
			GetComponent<ParticleSystem> ().Play ();

		}


	}


	void OnTriggerExit(Collider other){

		if (other.transform.root.gameObject.tag == "player") {
			GetComponent<ParticleSystem> ().Stop ();

		}

	}
}
