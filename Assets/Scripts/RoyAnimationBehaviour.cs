using UnityEngine;

[RequireComponent(typeof(Animator))]
public class RoyAnimationBehaviour : MonoBehaviour
{
    private Animator Anim;

    private float IdleKickTimer;
    private float TimeToIdleKick;
    private float IdleSitTimer;
    private const float TimeToIdleSit = 15f;

    #region Anim Name Hashes
    int Hash_BaseIdle;
    int Hash_IdleKick;
    int Hash_IdleSit;

    int Hash_TriggerIdleKick;
    int Hash_TriggerIdleSit;
    #endregion

    private void Awake()
    {
        Anim = GetComponent<Animator>();
        Hash_BaseIdle = Animator.StringToHash("Idle.BaseIdle");
        Hash_IdleKick = Animator.StringToHash("Idle.IdleKick");
        Hash_IdleSit = Animator.StringToHash("Idle.IdleSit");

        Hash_TriggerIdleKick = Animator.StringToHash("TriggerIdleKick");
        Hash_TriggerIdleSit = Animator.StringToHash("TriggerIdleSit");

        IdleKickTimer = IdleSitTimer = 0;
        TimeToIdleKick = 4.0f;
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo _animInfo = Anim.GetCurrentAnimatorStateInfo(0);
        // Only track time if not sitting
        if (_animInfo.fullPathHash != Hash_IdleSit)
        {
            IdleSitTimer += Time.deltaTime;
            IdleKickTimer += Time.deltaTime;

            // If Roy has idled too long, sit down
            if (IdleSitTimer >= TimeToIdleSit)
            {
                Anim.SetTrigger(Hash_TriggerIdleSit);
                // Reset timers for when he stands up next
                IdleKickTimer = 0;
                IdleSitTimer = 0;
            }
            // If not sitting and its time to kick
            else if (IdleKickTimer >= TimeToIdleKick)
            {
                Anim.SetTrigger(Hash_TriggerIdleKick);
                IdleKickTimer = 0;
                TimeToIdleKick = Random.Range(7, 12);
            }
        }

    }
}
