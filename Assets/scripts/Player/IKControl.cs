using UnityEngine;
using System;
using System.Collections;


public class IKControl : MonoBehaviour {
    private Player player;
    protected Animator animator;

    public bool ikActive = false;

    // Reference to Inventory — assign in Inspector to the player GameObject.
    public Inventory inventory;

    // The hand bone transform from the character's skeleton.
    public Transform rightHandBone;

    // Tracks the actual scene root of the currently held item (e.g. jointItemR)
    private Transform currentItemRoot;

    void Start()
    {
        player = GetComponentInParent<Player>();
        animator = player.Animator;
        inventory = player.InvManagement;
    }

    // Called by Inventory whenever a new item is equipped or unequipped.
    // Clears the tracked item root so LateUpdate re-evaluates it fresh.
    public void ClearItemRoot()
    {
        currentItemRoot = null;
    }

    // Called by Inventory — kept for API compatibility.
    public void SetGrabHandleOffset(Vector3 localPos, Quaternion localRot) { }

    // OnAnimatorIK drives the hand bone TO the IKGrabHandle world position.
    // The item is already parented to the hand bone, so as the hand moves to
    // the grab handle, the item moves with it.
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
                }
            }
            else
            {
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
            }
        }
    }

    // LateUpdate runs after IK has fully resolved.
    // Parent the item to the hand bone at a zero local offset.
    // The IKGrabHandle position on the item defines WHERE on the item the hand
    // grips — you position the item mesh relative to the grab handle inside the
    // prefab, not the other way around.
    void LateUpdate()
    {
        if (!ikActive || rightHandBone == null || inventory == null) return;

        GameObject handItem = inventory.GetHandItemInstance();
        if (handItem == null) { currentItemRoot = null; return; }

        Transform grabHandle = inventory.GetGrabHandle();
        if (grabHandle == null) return;

        // Find and cache the item root only once per equipped item
        if (currentItemRoot == null)
        {
            currentItemRoot = handItem.transform;
            while (currentItemRoot.parent != null && currentItemRoot.parent != rightHandBone)
                currentItemRoot = currentItemRoot.parent;

            currentItemRoot.SetParent(rightHandBone, false);
            currentItemRoot.localPosition = Vector3.zero;
            currentItemRoot.localRotation = Quaternion.identity;
        }

        // Calculate the grab handle's offset from itemRoot in local space
        Vector3 grabLocalPos = currentItemRoot.InverseTransformPoint(grabHandle.position);
        Quaternion grabLocalRot = Quaternion.Inverse(currentItemRoot.rotation) * grabHandle.rotation;

        // Shift itemRoot so the grab handle sits exactly at the hand bone origin
        currentItemRoot.localPosition = currentItemRoot.localRotation * grabLocalPos;
        currentItemRoot.localRotation = Quaternion.Inverse(grabLocalRot);
    }
}