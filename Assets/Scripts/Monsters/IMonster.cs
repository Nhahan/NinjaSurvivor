namespace Monsters
{
    public interface IMonster
    {
        float GetMonsterHp();

        void SetMonsterHp(float monsterHp);

        void TakeDamage(float damage);
    }
}
