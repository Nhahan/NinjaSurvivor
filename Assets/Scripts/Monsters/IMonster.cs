namespace Monsters
{
    public interface IMonster
    {
        void TakeDamage(float damage);

        public void StopMonster();

        public void ResumeMonster();
    }
}
