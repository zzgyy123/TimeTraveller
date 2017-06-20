using UnityEngine;
using System.Collections;

public class DoneHashIDs : MonoBehaviour
{
    // Here we store the hash tags for various strings used in our animators.
    public int dyingState;
    public int locomotionState;
    public int shoutState;
    public int deadBool;
    public int sneakingBool;
    public int shoutingBool;
    public int playerInSightBool;
    public int shotFloat;
    public int aimWeightFloat;
    public int angularSpeedFloat;
    public int openBool;

    public int speedFloat;
    public int directionFloat;
    public int readyPushBool;
    public int hitBoxBool;
    public int pushingState;
    public int readyPushBackState;


    void Awake()
    {
        dyingState = Animator.StringToHash("Base Layer.Dying");
        locomotionState = Animator.StringToHash("Base Layer.Locomotion");
        shoutState = Animator.StringToHash("Shouting.Shout");
        deadBool = Animator.StringToHash("Dead");
        speedFloat = Animator.StringToHash("Speed");
        sneakingBool = Animator.StringToHash("Sneaking");
        shoutingBool = Animator.StringToHash("Shouting");
        playerInSightBool = Animator.StringToHash("PlayerInSight");
        shotFloat = Animator.StringToHash("Shot");
        aimWeightFloat = Animator.StringToHash("AimWeight");
        angularSpeedFloat = Animator.StringToHash("AngularSpeed");
        openBool = Animator.StringToHash("Open");

        speedFloat = Animator.StringToHash("Speed");
        directionFloat = Animator.StringToHash("Direction");
        readyPushBool = Animator.StringToHash("ReadyPush");
        hitBoxBool = Animator.StringToHash("HitBox");
        pushingState = Animator.StringToHash("Base Layer.Pushing");
        readyPushBackState = Animator.StringToHash("Base Layer.ReadyPushBack");
    }
}
