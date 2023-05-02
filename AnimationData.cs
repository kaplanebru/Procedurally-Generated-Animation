using DG.Tweening;
using UnityEngine;

namespace _Scripts
{
    public enum BodyState
    {
        Mobile,
        Immobile
    }

    [CreateAssetMenu(menuName = "My Assets/Animation Data")]
    public class AnimationData : ScriptableObject
    {
        public Transform target;
    
        [Header("Body Parts")]
        public Transform head;
        public Transform body;
        public Transform[] legs;
        public Transform[] shoulders;
       

        [Header("Motion Values")]
        public float maxDistance = 5;
        public float rotAngle = 5f;
    
        [Header("Speed and Tween")]
        public float bodySpeed = 0.01f;
        public float jumpDuration = 0.3f;
        public float legDuration = 0.2f;
        public Ease releaseEase;

    }
}