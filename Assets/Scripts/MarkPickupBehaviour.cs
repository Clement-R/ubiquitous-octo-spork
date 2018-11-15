using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkPickupBehaviour : MonoBehaviour {

    public GameObject sfxPlayer;
    public AudioClip pickupSound;

	private void FixedUpdate() {
		transform.Rotate(Vector3.up, 5f);
	}

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
            GameObject sfx = Instantiate(sfxPlayer, transform.position, Quaternion.identity);
            AudioSource audio = sfx.GetComponent<AudioSource>();
            audio.PlayOneShot(pickupSound);
            Destroy(sfx, pickupSound.length + 0.1f);

            other.GetComponent<CharacterController>().AddMarker();
			Destroy(gameObject);
		}
	}
}
