using UnityEngine;
using System.Collections;

public class PathManager : MonoBehaviour {

	public Transform chest;
	public Transform player;
	public int pointsToCollect = 6;
	public float pointsDistance = 15.0F; //represents approximate radius

	public AudioClip gongSound;

    public bool chestFound;

    public int score = 0;

	private Compass compass;
	private Vector2 checkpoint;
	private AudioSource audioSource;

	private float volume = 0.45F;
	private int collectedPoints;
	private bool chestPoint;


	// Use this for initialization
	void Start () {
		Random.seed = (int)System.DateTime.Now.Ticks;

        chest.GetComponent<Chest>().pathManager = this;


		audioSource = GetComponent<AudioSource>();
		compass = GetComponent<Compass> ();

		collectedPoints = 0;
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

                audioSource.PlayOneShot (gongSound, volume);
                generateNewPoint ();
            }


        } else if (Vector2.Distance(checkpoint, playerCoords) < 4.0F) {
			audioSource.PlayOneShot (gongSound, volume);
			collectedPoints++;

			if (collectedPoints == pointsToCollect) {
				chestPoint = true;
				generateChest ();
				collectedPoints = 0;
			} else {
				generateNewPoint ();
			}

		}



	}



	private Vector2 positionInCircle(){
		Vector3 playerPosition = player.position;

		float x, z, absoluteX, absoluteZ;

		absoluteX = Random.Range(pointsDistance * (-1), pointsDistance);
		absoluteZ = Mathf.Sqrt(pointsDistance * pointsDistance - absoluteX * absoluteX);

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

}
