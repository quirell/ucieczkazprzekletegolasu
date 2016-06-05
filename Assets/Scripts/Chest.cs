using UnityEngine;
using System.Collections;

public class Chest : MonoBehaviour {

	public PathManager pathManager;
    public GameObject explosion;


	void OnTriggerEnter(Collider other) {


		//Debug.Log(other.transform.root.gameObject.tag);

		if (other.transform.root.gameObject.tag == "player") {

            //TODO: add sth to do?

            // hide chest
            this.gameObject.SetActive(false);

            // play explosion
            playExplosion();

            pathManager.chestFound = true;
		}



	}


    private void playExplosion() {
        explosion.transform.position = this.transform.position;
        explosion.SetActive(true);
        explosion.GetComponentInChildren<ParticleSystem> ().Stop();
        explosion.GetComponentInChildren<ParticleSystem> ().transform.localPosition = new Vector3(0,0,0);
        explosion.GetComponentInChildren<ParticleSystem> ().Emit (1);

        // ... nie chce przestac
        // TODO: emit tylko raz
        // TODO: wogole nie dziala, particles zostają w starym miejscu...

        
    }
}
