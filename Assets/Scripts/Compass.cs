using UnityEngine;
using System.Collections;

public class Compass : MonoBehaviour {

	public Transform player;
	public Texture compassBackground;
	public Texture pointer;

	public Transform chest;

	void OnGUI () {
		GUI.DrawTexture (new Rect (0, 0, 120, 120), compassBackground);


		//draw pointer
		float absoluteAngle, angle, playerAngle;

		Vector3 dir = chest.transform.position - player.transform.position;
		absoluteAngle = Mathf.Atan2 (dir.x, dir.z) * Mathf.Rad2Deg;
		playerAngle = player.eulerAngles.y;

		angle = playerAngle - absoluteAngle;
		angle = 360 - angle;

		Matrix4x4 matrixBackup = GUI.matrix;
		GUIUtility.RotateAroundPivot(angle, new Vector2(60, 60));

		GUI.DrawTexture (new Rect(0, 0, 120, 120), pointer);
	
		GUI.matrix = matrixBackup;

	}



}
