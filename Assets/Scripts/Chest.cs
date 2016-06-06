using UnityEngine;
using System.Collections;

public class Chest : MonoBehaviour {

	public PathManager pathManager;
    public GameObject explosionPrefab;

	// Store reference to the particular instance
	GameObject explosionInstance;
	private float explosionLifetime = 3.0f;


	void OnTriggerEnter(Collider other) {
		explosionInstance = Instantiate (explosionPrefab, Vector3.zero, Quaternion.identity) as GameObject;

		//Debug.Log(other.transform.root.gameObject.tag);

		if (other.transform.root.gameObject.tag == "player") {

			// hide chest
            this.gameObject.SetActive(false);

            // play explosion
            playExplosion();

            pathManager.chestFound = true;
		}

	}

	private void playExplosion() {
		explosionInstance.transform.position = this.transform.position;

		explosionInstance.SetActive(true);
		explosionInstance.GetComponentInChildren<ParticleSystem> ().Stop();
		explosionInstance.GetComponentInChildren<ParticleSystem> ().transform.localPosition = new Vector3(0,0,0);
		explosionInstance.GetComponentInChildren<ParticleSystem> ().Emit (1);

		Destroy (explosionInstance, explosionLifetime);
      
    }


}
