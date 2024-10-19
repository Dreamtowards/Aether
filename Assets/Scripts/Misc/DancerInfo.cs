using System;
using UnityEngine;

namespace Aether
{
    public class DancerInfo : MonoBehaviour
    {
        public Avatar avatar;

        private void OnEnable()
        {
            var animator = GetComponentInParent<Animator>();
            if (animator)
                animator.avatar = avatar;
        }
    }
}