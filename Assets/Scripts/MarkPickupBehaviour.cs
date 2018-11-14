using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkPickupBehaviour : MonoBehaviour {
	private void FixedUpdate() {
		transform.Rotate(Vector3.up, 5f);
	}

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			other.GetComponent<CharacterController>().AddMarker();
			Destroy(gameObject);
		}
	}
}
