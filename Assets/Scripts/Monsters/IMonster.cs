using UnityEngine;

namespace Monsters
{
    public interface IMonster
    {
        public void TakeDamage(float damage);

        public void StartKnockback(Vector3 knockbackDirection);

        public void StopMonster();

        public void ResumeMonster();
    }
}