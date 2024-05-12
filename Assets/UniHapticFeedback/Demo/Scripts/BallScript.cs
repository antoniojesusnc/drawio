using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.name == "Platform (8)") {
            this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0.0f, 20.0f), ForceMode2D.Impulse);
            HapticFeedback.DoHaptic(HapticFeedback.NotificationType.Success);
        } else {
            HapticFeedback.DoHaptic(HapticFeedback.HapticForce.Medium);
        }
    }
}
