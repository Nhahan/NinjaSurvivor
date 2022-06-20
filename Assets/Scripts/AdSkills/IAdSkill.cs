using UnityEngine;

namespace AdSkills
{
    internal interface IAdSkill
    {
        void Awake() { }

        void Start() { }

        void Update() { }

        void OnTriggerEnter2D(Collider2D coll) { }

        void IsAvailable() { }
    }
}
