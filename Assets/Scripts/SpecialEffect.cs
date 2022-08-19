using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEffect : MonoBehaviour
{
    public int EffectType = 0;
    public Animator anim;

    private void Start()
    {
        anim.SetInteger("Effect Type", EffectType);
    }
    public void Update()
    {
        if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
        {
            Destroy(gameObject);
        }
    }
}
