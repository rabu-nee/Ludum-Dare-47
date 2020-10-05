using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Tools;

public class Player : MonoBehaviour {
    public float movementSpeed = 5f;
    public float maxSwimTimeWithoutLifebuoy = 10f;
    public float lifebuoySearchRange = 0.1f;

    private Rigidbody rb;
    private Lifebuoy lifebuoy;

    private float horizontalMovement;
    private float verticalMovement;
    [SerializeField]
    private float speedMultiplier = 1f;

    private float soundTimer;

    private Coroutine DrownTimer;
    private bool canMove;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        lifebuoy = GetComponentInChildren<Lifebuoy>();
        lifebuoy.Drown += StartDrownTimer;
    }

    private void OnEnable() {
        if (lifebuoy != null)
            lifebuoy.Drown += StartDrownTimer;
        UIManager.StartG += EnableMovement;
        GameManager.End += DisableMovement;
    }

    private void OnDisable() {
        lifebuoy.Drown -= StartDrownTimer;
        UIManager.StartG -= EnableMovement;
        GameManager.End -= DisableMovement;
    }

    private void DisableMovement(EndState endState) {
        canMove = false;
    }

    private void EnableMovement() {
        canMove = true;
    }

    // Update is called once per frame
    void Update() {
        if (!canMove) return;

        horizontalMovement = Input.GetAxis("Horizontal");
        verticalMovement = Input.GetAxis("Vertical");

        if (horizontalMovement != 0 || verticalMovement != 0) {
            float angle = Mathf.Atan2(horizontalMovement, verticalMovement);
            transform.rotation = Quaternion.EulerAngles(0, angle, 0);

            if (soundTimer <= 0) {

                //play movement sounds
                Puppet.Sound.SoundManager.Self.PlaySound("Bug_Swimming");
                soundTimer = 0.3f;
            }
            else {
                soundTimer -= Time.deltaTime;
            }
        }

        if (Input.GetButtonDown("Fire1")) {
            if (lifebuoy.GetBitesLeft() > 0) {
                Vector2 boost = lifebuoy.GetSpeedBoost();
                if (boost.x != 1 && speedMultiplier == 1f) {
                    speedMultiplier = boost.x;
                    StartCoroutine(RevertSpeedboostAfterTime(boost.y));
                }
            }
            else {
                //no lifebuoy, searching for another
                //casting ray
                RaycastHit[] hit = Physics.SphereCastAll(transform.position, lifebuoySearchRange, Vector3.up);
                for (int i = 0; i < hit.Length; i++) {
                    if (hit[i].collider.CompareTag("FruitLoop")) {
                        //if success, set bites
                        hit[i].collider.gameObject.SetActive(false);
                        speedMultiplier = 1f;
                        lifebuoy.ResetBites(hit[i].collider.gameObject);
                        StopCoroutine(DrownTimer);
                        UIManager.HideTimer();

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
            Puppet.Sound.SoundManager.Self.PlaySound("Bug_Drowning");
            GameManager.TriggerGameEnd(Tools.EndState.GAME_OVER);
            transform.DOMoveY(1f, 1f);
            UIManager.HideTimer();
        }
    }

    private void FixedUpdate() {
        if (horizontalMovement != 0 || verticalMovement != 0)
        {
            //rb.velocity =  Vector3.Normalize(new Vector3(horizontalMovement, rb.velocity.y, verticalMovement)) * movementSpeed * speedMultiplier * Time.deltaTime;
            var targetVelocity = Vector3.Normalize(new Vector3(horizontalMovement, 0, verticalMovement));
            targetVelocity *= movementSpeed * speedMultiplier;
            var velocity = rb.velocity;
            var velocityChange = (targetVelocity - velocity);
            rb.AddForce(velocityChange, ForceMode.VelocityChange);
            //rb.AddForce(Vector3.Normalize(new Vector3(horizontalMovement, 0, verticalMovement)) * movementSpeed * speedMultiplier, ForceMode.Impulse);
            //rb.MovePosition(transform.position + Vector3.Normalize(new Vector3(horizontalMovement, 0, verticalMovement)) * movementSpeed * speedMultiplier * Time.deltaTime);
        }
            
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, lifebuoySearchRange);
    }
#endif
}
