using System.Collections;
using Monsters;
using Status;
using UnityEngine;

namespace ApSkills
{
    public class LightningStrike : MonoBehaviour
    {
        [SerializeField] private float damageMultiplier = 1.5f;

        private Player _player;
        public Vector3 target;
        private ParticleSystem _system;
        private static readonly ParticleSystem.Particle[] Particles = new ParticleSystem.Particle[50];

        private float _baseSkillDamage = 7.5f;
        private float _skillLevelMultiplier = 0.2f;
        private float _damage;

        private void Start()
        {
            if (_system == null)
            {
                _system = GetComponent<ParticleSystem>();
            }

            if (_system == null) {
                this.enabled = false;
            } else {
                _system.Play();
            }

            _player = GameManager.Instance.GetPlayer();

            target = transform.position - new Vector3(0, 8, 0);
            
            var skillLevelBonus = _skillLevelMultiplier * _player.BasicStar.CalculateFinalValue();
            _damage = _player.Damage() * damageMultiplier * skillLevelBonus + _baseSkillDamage + Random.Range(-1, 6);
            
            Destroy(gameObject, 0.45f);
        }

        private void FixedUpdate()
        {
            var count = _system.GetParticles(Particles);
            
            for (var i = 0; i < count; i++)
            {
                ParticleSystem.Particle particle = Particles[i];

                Vector3 v1 = _system.transform.TransformPoint(particle.position);
                Vector3 v2 = target;
                
                Vector3 targetPosition = (v2 - v1) *  (particle.remainingLifetime / particle.startLifetime);
                particle.position = _system.transform.InverseTransformPoint(v2 - targetPosition);
                Particles[i] = particle;
            }

            _system.SetParticles(Particles, count);
        }

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if (!(coll.CompareTag("Enemy") || coll.CompareTag("Boss"))) return;

            var monster = coll.gameObject.GetComponent<IMonster>();

            var normal = (coll.gameObject.transform.position - transform.position).normalized;
            monster.TakeDamage(_damage);
            monster.StartKnockback(normal);
        }
    }
}