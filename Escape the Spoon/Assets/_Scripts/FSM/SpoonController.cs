using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tools;
using UnityEngine;

public class SpoonController : StatefulMonoBehaviour<SpoonController> {
    public Transform
        player,
        spoonTipPos;

    public Animator animator;
    public enum SpoonStates { IDLE, SCOOP, SLOW, SPLASH };

    public float maxCooldown = 6f;
    public float minCoolDown = 3f;
    public bool cooldownActive;

    public Bowl bowl;
    public float fluidDecrease = 0.1f;
    public float spoonTipRadius;

    private void Start() {
        fsm = new FSM<SpoonController>();
    }

    private void StartGame() {
        fsm.Configure(this, new SpoonIdle());
    }

    private void OnEnable() {
        UIManager.StartG += StartGame;
        GameManager.End += OnGameEnd;
    }

    private void OnDisable() {
        UIManager.StartG -= StartGame;
        GameManager.End -= OnGameEnd;
    }

    private void OnGameEnd(EndState endState) {
        this.enabled = false;
    }

    public void SetAnimation(SpoonStates state) {
        animator.SetInteger("state", (int)state);
    }

    public void RevertToIdle() {
        ChangeState(new SpoonIdle());
    }

    public int ChangeAttack() {
        int randomAttack = UnityEngine.Random.Range(0, 5);
        switch ((SpoonStates)randomAttack) {
            case SpoonStates.SCOOP:
                ChangeState(new SpoonScoop());
                break;
            case SpoonStates.SLOW:
                ChangeState(new SpoonSlow());
                break;
            case SpoonStates.SPLASH:
                ChangeState(new SpoonSplash());
                break;
        }
        bowl.DecreaseFluidLevel(fluidDecrease);
        return randomAttack;
    }

    public delegate void FruitLoopEaten();
    public static FruitLoopEaten Eaten;

    public bool IsPlayerOnSpoon() {
        RaycastHit[] hits = Physics.SphereCastAll(spoonTipPos.position, 1f, Vector3.up);
        Debug.Log("Checking if Player is on Spoon | detected collisions: " + hits.Length);
        for (int i = 0; i<hits.Length; i++) {
            if (hits[i].collider.CompareTag("Player")) {
                return true;
            }
            else if(hits[i].collider.CompareTag("FruitLoop")) {
                hits[i].collider.gameObject.SetActive(false);
                Eaten?.Invoke();
                Puppet.Sound.SoundManager.Self.PlaySound("Human_Eating");
            }
        }
        return false;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        UnityEditor.Handles.DrawWireDisc(spoonTipPos.position, Vector3.up, spoonTipRadius);
    }
#endif

    public Vector3 GetRandomPos() {
        float radius = bowl.GetInitTopRadius();
        Vector3 randomRadiusPos = (UnityEngine.Random.insideUnitCircle * radius);
        randomRadiusPos.y = 0;
        return randomRadiusPos;
    }
}
