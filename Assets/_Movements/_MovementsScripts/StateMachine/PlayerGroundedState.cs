using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is a concrete state
public class PlayerGroundedState : PlayerBaseState, IRootState
{
    public PlayerGroundedState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory)
    {
        IsRootState = true;
    }

    public void HandleGravity()
    {
        Ctx.CurrentMovementY = Ctx.Gravity;
        Ctx.AppliedMovementY = Ctx.Gravity;
    }

    public override void EnterState()
    {
        InitializeSubState();
        HandleGravity();
        //Debug.Log("Enter grounded state!");
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        // Debug.Log("Exit grounded state!");
    }

    public override void InitializeSubState()
    {
        if (!Ctx.IsMovementPressed && !Ctx.IsRunPressed)
        {
            SetSubState(Factory.Idle());
        }
        else if (Ctx.IsMovementPressed && !Ctx.IsRunPressed)
        {
            SetSubState(Factory.Walk());
        }
        else
        {
            SetSubState(Factory.Run());
        }
    }

    public override void CheckSwitchStates()
    {
        // if player is grounded & jump is pressed, switch to jump state
        if (Ctx.IsJumpPressed && !Ctx.RequireNewJumpPress)
        {
            SwitchState(Factory.Jump());
        }
        else if (!Ctx.CharacterController.isGrounded)
        {
            SwitchState(Factory.Fall());
        }
    }
}