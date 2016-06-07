using UnityEngine;
using System.Collections;

public class PathManager : MonoBehaviour {

	public Transform chest;
	public Transform player;

    public Light light;

    public AudioClip gongSound;
	public AudioClip explosionSound;
    private float gongVolume = 0.7F;
	private float explosionVolume = 0.9F;

    public int pointsToCollect = 6;
	public float pointsDistance = 15.0F; //represents approximate radius
    private int collectedCheckpoints;

    public bool chestFound;

    public int chestToCollect = 6;
    private bool chestPoint;
    public int score = 0;

	private Compass compass;
	private Vector2 checkpoint;
	private AudioSource audioSource;

    private GameObject endGameObj;
    public Transform endLight;
    private bool endGame = false;

    public GUIStyle scoreStyle;

	// Use this for initialization
	void Start () {
		Random.seed = (int)System.DateTime.Now.Ticks;

        chest.GetComponent<Chest>().pathManager = this;

        endLight.gameObject.SetActive (false);

		audioSource = GetComponent<AudioSource>();
		compass = GetComponent<Compass> ();

		collectedCheckpoints = 0;
		chestPoint = false;

		chest.localScale = new Vector3(0.2F, 0.2F, 0.2F);
		chest.gameObject.SetActive (false);

		generateNewPoint ();

	
	}
	
	// Update is called once per frame
	void Update () {
	
		Vector2 playerCoords = new Vector2(player.position.x, player.position.z);

		//Debug.Log (Vector2.Distance(checkpoint, playerCoords));

        if (chestPoint) {
            if (chestFound) {
                chestPoint = false;
                score++;
                chestFound = false;

				audioSource.PlayOneShot (explosionSound, explosionVolume);

                //lower red and green in light
                light.color -= new Color (0.0f, 0.12f, 0.0F);
                light.intensity -= 0.4f;
                light.transform.Rotate(-10,0,0);
//                light.color -= new Color (0.0f, 0.22f, 0.0F);
//                light.intensity -= 0.85f;
//                light.transform.Rotate (-25, 0, 0);

                if (score == chestToCollect) {
                    generateGameEnd ();
                } else {
                    generateNewPoint ();
                }
            }


        } else if (Vector2.Distance (checkpoint, playerCoords) < 4.0F) {
            audioSource.PlayOneShot (gongSound, gongVolume);
            collectedCheckpoints++;

            if (collectedCheckpoints == pointsToCollect) {
                chestPoint = true;
                generateChest ();
                collectedCheckpoints = 0;
            } else {
                generateNewPoint ();
            }

        } else if (endGame) {
            // endLight.position = new Vector3 (endLight.transform.position.x, 
            //    player.position.y + 2, endLight.transform.position.z);
        }



	}


    void OnGUI(){
        
        GUI.Label (new Rect (Screen.width - 90, 20, 60, 25), "Score " + score.ToString(), scoreStyle);
    }


    private Vector2 positionInCircle(){
        return positionInCircle (1.0F);
    }

    private Vector2 positionInCircle(float scalar){
		Vector3 playerPosition = player.position;

		float x, z, absoluteX, absoluteZ;

        float relPointDistance = pointsDistance * scalar;

        absoluteX = Random.Range(relPointDistance * (-1), relPointDistance);
        absoluteZ = Mathf.Sqrt(relPointDistance * relPointDistance - absoluteX * absoluteX);

		absoluteZ *= Random.Range (0.0F, 1.0F) > 0.5 ? 1 : (-1);

		x = absoluteX + playerPosition.x;
		z = absoluteZ + playerPosition.z;

		return new Vector2 (x, z);
	}


	private void generateNewPoint(){
		checkpoint = positionInCircle();
		compass.setDestination(checkpoint);
	}

	private void generateChest(){
		Vector2 chestPosition = positionInCircle();

		chest.position = new Vector3(chestPosition.x, 40, chestPosition.y);
        chest.gameObject.SetActive (true);

        compass.setDestination(chestPosition);
	}


    private void generateGameEnd(){

        Vector2 endPosition = positionInCircle(3.0F);
        compass.setDestination(endPosition);


        // generate end light and stuff
//        endGameObj = new GameObject("The Light");
//        Light endLight = endGameObj.AddComponent<Light>();
//        endLight.color = Color.white;
//        endLight.type = LightType.Spot;
//        endLight.range = 100;
//        endLight.spotAngle = 100;
//        endLight.intensity = 2.7F;
//        //endLight.flare = FlareLayer ();
//        endGameObj.transform.position = new Vector3(endPosition.x, 20, endPosition.y);
        endLight.gameObject.SetActive(true);
        endLight.position = new Vector3(endPosition.x, 20, endPosition.y);
        endLight.GetComponentInChildren<ParticleSystem> ().Play();

        endGame = true;

        //TODO: komponent ParticleSystem jest wyzej niz powinein byc....

    }

}
