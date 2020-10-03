using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public float movementSpeed = 100f;

    [SerializeField]
    private Transform SpriteGroup;
    private Rigidbody rb;

    private float horizontalMovement;
    private float verticalMovement;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        SpriteGroup.LookAt(Camera.main.transform);
    }

    // Update is called once per frame
    void Update() {
        horizontalMovement = Input.GetAxis("Horizontal");
        verticalMovement = Input.GetAxis("Vertical");
    }

    private void FixedUpdate() {
        //rb.velocity = Vector3.Normalize(new Vector3(horizontalMovement, rb.velocity.y, verticalMovement)) * movementSpeed * Time.deltaTime;
        rb.MovePosition(transform.position + Vector3.Normalize(new Vector3(horizontalMovement, 0, verticalMovement)) * movementSpeed * Time.deltaTime);
    }
}
