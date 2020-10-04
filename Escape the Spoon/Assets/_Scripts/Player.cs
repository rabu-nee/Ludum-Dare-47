using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public float movementSpeed = 100f;
    [SerializeField]
    private Transform SpriteGroup;
    private Rigidbody rb;
    private Lifebuoy lifebuoy;

    private float horizontalMovement;
    private float verticalMovement;
    private float speedMultiplier = 1f;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        lifebuoy = GetComponentInChildren<Lifebuoy>();
        SpriteGroup.LookAt(Camera.main.transform);
    }

    // Update is called once per frame
    void Update() {
        horizontalMovement = Input.GetAxis("Horizontal");
        verticalMovement = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Fire1") && speedMultiplier == 1f) {
            Vector2 boost = lifebuoy.GetSpeedBoost();
            if (boost.x != 1) {
                speedMultiplier = boost.x;
                StartCoroutine(RevertSpeedboostAfterTime(boost.y));
            }
        }
    }

    private IEnumerator RevertSpeedboostAfterTime(float time) {
        Debug.Log("Boost start");
        yield return new WaitForSeconds(time);
        speedMultiplier = 1f;
        Debug.Log("Boost over");
    }

    private void FixedUpdate() {
        rb.MovePosition(transform.position + Vector3.Normalize(new Vector3(horizontalMovement, 0, verticalMovement)) * movementSpeed * speedMultiplier * Time.deltaTime);
    }
}
