namespace Monsters
{
    public interface IMonster
    {
        void SetMonsterHp(float monsterHp);

        void TakeDamage(float damage);

        public void StopMonster();

        public void ResumeMonster();
    }
}
