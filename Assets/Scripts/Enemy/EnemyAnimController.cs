using System;
using UnityEngine;

public class EnemyAnimController : MonoBehaviour
{
    [SerializeField] Animator anim;
    AnimationsTransition currentAnim;


    public void PlayAnimation(AnimationsTransition newAnim)
    {
        if (currentAnim == newAnim) return;

        anim.SetBool(currentAnim.ToString(), false);
        currentAnim = newAnim;
        anim.SetBool(currentAnim.ToString(), true);
    }
}

public enum AnimationsTransition
{
    Alert,
    Perseguir,
    Patrullar
}