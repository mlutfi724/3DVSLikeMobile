using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

// This class is a concrete state
public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory) { }

    public override void EnterState()
    {
        //Debug.Log("Enter walk state!");
        Ctx.Animator.SetBool(Ctx.IsWalkingHash, true);
        Ctx.Animator.SetBool(Ctx.IsRunningHash, false);
        PlaySFX();
    }

    public override void UpdateState()
    {
        Ctx.AppliedMovementX = Ctx.CurrentMovementInput.x * Ctx.MoveSpeed;
        Ctx.AppliedMovementZ = Ctx.CurrentMovementInput.y * Ctx.MoveSpeed;
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        //Debug.Log("Exit walk state!");
        StopSFX();
    }

    public override void InitializeSubState()
    { }

    public override void CheckSwitchStates()
    {
        if (!Ctx.IsMovementPressed)
        {
            SwitchState(Factory.Idle());
        }
        else if (Ctx.IsMovementPressed && Ctx.IsRunPressed)
        {
            SwitchState(Factory.Run());
        }
    }

    private void PlaySFX()
    {
        MMSoundManagerPlayOptions options;
        options = MMSoundManagerPlayOptions.Default;
        options.Loop = true;
        options.Volume = 0.5f;
        options.ID = 270421;

        Ctx.WalkSFXAudioSource = MMSoundManagerSoundPlayEvent.Trigger(Ctx.WalkSFX, options);
    }

    private void StopSFX()
    {
        MMSoundManagerSoundControlEvent.Trigger(MMSoundManagerSoundControlEventTypes.Free, 270421, Ctx.WalkSFXAudioSource);
    }
}