using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement;


public class Chest : MonoBehaviour {

	public PathManager pathManager;
    public GameObject explosionPrefab;
    public float timeForAction = 0.3f;
    public GUIStyle textStyle;

	// Store reference to the particular instance
	GameObject explosionInstance;
	private float explosionLifetime = 3.0f;
    private bool actionTime = false;
    private float timeLeft;

    private KeyCode[] keyList = {KeyCode.Y, KeyCode.Space, KeyCode.N, KeyCode.Backspace, KeyCode.C};
    private KeyCode actionToMake;

    //TODO: probably should be nit in this class
    static public bool success = true;  

    //TODO: nie tak
    private float gameOverTimeout = 3.0f;

    void Update () {
        if (actionTime) {
            
            if (Input.GetKeyDown (actionToMake)) {
                // unactivate chest
                this.gameObject.SetActive(false);
                actionTime = false;

                pathManager.chestFound = true;
            }

            timeLeft -= Time.deltaTime;
            if (timeLeft < 0) {
                success = false;
                gameOver ();
            }
        }
        if (!success) {
            gameOverTimeout -= Time.deltaTime;
            if (gameOverTimeout < 0) {
                SceneManager.LoadScene ("ending");
            }
        }
                
    }

    void OnGUI(){

        if (actionTime) {
            if (success) {
                
                GUI.Label (new Rect (40, Screen.height / 2 - 30, Screen.width - 40, 60), 
                    "Press " + actionToMake + "!", textStyle);
            }else{
                GUI.Label (new Rect (40, Screen.height / 2 - 30, Screen.width - 40, 60), 
                    "GAME OVER!", textStyle);
            }

        }
    }

	void OnTriggerEnter(Collider other) {
		explosionInstance = Instantiate (explosionPrefab, Vector3.zero, Quaternion.identity) as GameObject;

		//Debug.Log(other.transform.root.gameObject.tag);

		if (other.transform.root.gameObject.tag == "player") {

            if (!actionTime) {
                // hide chest
                this.gameObject.transform.localScale = Vector3.zero;
                // play explosion
                playExplosion();

                int keyInd = Random.Range(0, keyList.Length);
                actionToMake = keyList[keyInd];
                timeLeft = timeForAction;
                actionTime = true;
            }

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


    private void gameOver(){

        //wiem ze brzydkie
        explosionInstance = Instantiate (explosionPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        explosionInstance.transform.position = pathManager.player.position;

        pathManager.player.gameObject.SetActive (false);

        explosionInstance.SetActive(true);
        explosionInstance.GetComponentInChildren<ParticleSystem> ().Stop();
        explosionInstance.GetComponentInChildren<ParticleSystem> ().transform.localPosition = new Vector3(0,0,0);
        explosionInstance.GetComponentInChildren<ParticleSystem> ().Emit (1);

        Destroy (explosionInstance, explosionLifetime);

        //SceneManager.LoadScene("ending");
    }

}
