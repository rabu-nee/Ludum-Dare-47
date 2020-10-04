using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public float movementSpeed = 5f;
    public float maxSwimTimeWithoutLifebuoy = 10f;

    [SerializeField]
    private Transform SpriteGroup;
    private Rigidbody rb;
    private Lifebuoy lifebuoy;

    private float horizontalMovement;
    private float verticalMovement;
    private float speedMultiplier = 1f;

    private Coroutine DrownTimer;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        lifebuoy = GetComponentInChildren<Lifebuoy>();
        lifebuoy.Drown += StartDrownTimer;

        SpriteGroup.LookAt(Camera.main.transform);
    }

    private void OnEnable() {
        if (lifebuoy != null)
            lifebuoy.Drown += StartDrownTimer;
    }

    private void OnDisable() {
        lifebuoy.Drown -= StartDrownTimer;
    }

    // Update is called once per frame
    void Update() {
        horizontalMovement = Input.GetAxis("Horizontal");
        verticalMovement = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Fire1")) {
            if (lifebuoy.GetBitesLeft() > 0) {
                Vector2 boost = lifebuoy.GetSpeedBoost();
                if (boost.x != 1) {
                    speedMultiplier = boost.x;
                    StartCoroutine(RevertSpeedboostAfterTime(boost.y));
                }
            }
            else {
                //no lifebuoy, searching for another
                //casting ray
                RaycastHit[] hit = Physics.RaycastAll(transform.position, transform.forward);
                for (int i = 0; i < hit.Length; i++) {
                    if (hit[i].collider.CompareTag("FruitLoop")) {
                        //if success, set bites
                        hit[i].collider.gameObject.SetActive(false);
                        lifebuoy.ResetBites();
                        StopCoroutine(DrownTimer);
                        break;
                    }
                }
            }
        }
    }

    private IEnumerator RevertSpeedboostAfterTime(float time) {
        Debug.Log("Boost start");
        yield return new WaitForSeconds(time);
        speedMultiplier = 1f;
        Debug.Log("Boost over");
    }

    private void StartDrownTimer() {
        speedMultiplier = 0.5f;
        DrownTimer = StartCoroutine(SetTimer());
    }

    private IEnumerator SetTimer() {
        float timer = maxSwimTimeWithoutLifebuoy;
        while (timer > 0) {
            timer -= Time.deltaTime;
            UIManager.SetTimer(timer);
            yield return new WaitForEndOfFrame();
        }
        //check if player found a lifebuoy
        if (lifebuoy.GetBitesLeft() <= 0) {
            GameManager.TriggerGameEnd(Tools.EndState.GAME_OVER);
        }
    }

    private void FixedUpdate() {
        rb.MovePosition(transform.position + Vector3.Normalize(new Vector3(horizontalMovement, 0, verticalMovement)) * movementSpeed * speedMultiplier * Time.deltaTime);
    }
}
