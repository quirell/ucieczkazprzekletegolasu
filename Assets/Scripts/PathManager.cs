using UnityEngine;
using System.Collections;

public class PathManager : MonoBehaviour {

	public Transform chest;

	// Use this for initialization
	void Start () {



		chest.position = new Vector3(20, 10, 20);
		chest.localScale = new Vector3(0.2F, 0.2F, 0.2F);


	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
