/*
=============================================================================================
BasicMotionsAnimatorStateChanger.cs

This script is needed for the "BasicMotionsCharacterController.cs" script to work properly.

This script changes CharacterState value in BasicMotionsCharacterController.cs script when
the Animator enters an Animator Controller State with this script.

https://www.keviniglesias.com/
support@keviniglesias.com
=============================================================================================
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEngine;

namespace KevinIglesias
{
    public class BasicMotionsAnimatorStateChanger : StateMachineBehaviour
    {
        [SerializeField]
        private CharacterState newState;

        private BasicMotionsCharacterController controller;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // Cache the controller reference when entering the state
            if (controller == null)
            {
                // First try to get from parent
                if (animator.transform.parent != null)
                {
                    controller = animator.transform.parent.GetComponent<BasicMotionsCharacterController>();
                }

                // If not found on parent, try on the same GameObject
                if (controller == null)
                {
                    controller = animator.GetComponent<BasicMotionsCharacterController>();
                }
            }
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (controller != null)
            {
                controller.ChangeState(newState);
            }
        }
    }
}
