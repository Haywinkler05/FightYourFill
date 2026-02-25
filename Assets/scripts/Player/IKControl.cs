using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class IKControl : MonoBehaviour {

    protected Animator animator;

    public bool ikActive = false;

    // Reference to Inventory — assign in Inspector to the player GameObject.
    public Inventory inventory;

    // The hand bone transform from the character's skeleton.
    // Assign this in the Inspector — find it by expanding your character's
    // armature in the Hierarchy and locating the right hand bone.
    public Transform rightHandBone;

    void Start ()
    {
        animator = GetComponent<Animator>();
    }

    void OnAnimatorIK()
    {
        if (animator)
        {
            if (ikActive)
            {
                Transform grabHandle = inventory != null ? inventory.GetGrabHandle() : null;

                if (grabHandle != null)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                    animator.SetIKPosition(AvatarIKGoal.RightHand, grabHandle.position);
                    animator.SetIKRotation(AvatarIKGoal.RightHand, grabHandle.rotation);

                    animator.SetLookAtWeight(1);
                    animator.SetLookAtPosition(grabHandle.position);
                }
            }
            else
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
                animator.SetLookAtWeight(0);
            }
        }
    }

    // LateUpdate runs after IK has fully resolved for the frame.
    // We parent the item to the hand bone on the first frame it appears so
    // Unity handles all positioning automatically. The IKGrabHandle's local
    // position and rotation within the prefab then controls where the hand
    // grips the item — move the handle in the prefab to adjust the grip.
    void LateUpdate()
    {
        if (!ikActive || rightHandBone == null || inventory == null) return;

        GameObject handItem = inventory.GetHandItemInstance();
        if (handItem == null) return;

        // Only reparent once — when the item is first equipped or swapped
        if (handItem.transform.parent != rightHandBone)
        {
            Transform grabHandle = inventory.GetGrabHandle();
            if (grabHandle == null) return;

            // Parent to the hand bone so the item moves with the hand automatically
            handItem.transform.SetParent(rightHandBone, false);

            // Position the item so the grab handle sits exactly at the hand bone origin.
            // worldToLocalMatrix converts the handle's world offset into hand-bone local space.
            handItem.transform.localPosition = -grabHandle.localPosition;
            handItem.transform.localRotation = Quaternion.Inverse(grabHandle.localRotation);
        }
    }
}