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
        // private Animator _animator;
        public Transform target;
        private ParticleSystem _system;
        private static ParticleSystem.Particle[] particles = new ParticleSystem.Particle[1000];

        private float _baseSkillDamage = 1.5f;
        private float _skillLevelMultiplier = 0.25f;
        private float _damage;
        
        int count;
        void FixedUpdate(){
		
            count = _system.GetParticles(particles);

            for (int i = 0; i < count; i++)
            {
                ParticleSystem.Particle particle = particles[i];

                Vector3 v1 = _system.transform.TransformPoint(particle.position);
                Vector3 v2 = target.transform.position;
                
                Vector3 tarPosi = (v2 - v1) *  (particle.remainingLifetime / particle.startLifetime);
                particle.position = _system.transform.InverseTransformPoint(v2 - tarPosi);
                particles[i] = particle;
            }

            _system.SetParticles(particles, count);
        }

        private void Start()
        {
            if (_system == null)
                _system = GetComponent<ParticleSystem>();

            if (_system == null){
                this.enabled = false;
            }else{
                _system.Play();
            }
            
            _player = GameManager.Instance.GetPlayer();
            // _animator = GetComponent<Animator>();
            
            var skillLevelBonus = _skillLevelMultiplier * _player.BasicStar.CalculateFinalValue();
            _damage = _player.Damage() * damageMultiplier * skillLevelBonus + _baseSkillDamage;
            
            // StartCoroutine(BeforeDestroy(_animator.GetCurrentAnimatorStateInfo(0).length));
        }

        private void OnTriggerEnter2D(Collider2D coll)
        {
            if (!coll.CompareTag("Enemy")) return;

            var monster = coll.gameObject.GetComponent<IMonster>();
            var target = coll.transform;

            var normal = (coll.gameObject.transform.position - transform.position).normalized;
            monster.TakeDamage(_damage);
            monster.StartKnockback(normal);
        }
        
        private IEnumerator BeforeDestroy(float second)
        {
            yield return new WaitForSeconds(second);
            Destroy(gameObject);
        }
    }
}