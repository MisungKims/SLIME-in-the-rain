/**
 * @brief Ω∫≈» ≈¨∑°Ω∫
 * @author ±ËπÃº∫
 * @date 22-06-25
 */

[System.Serializable]
public class Stats
{
    public float HP;
    public float coolTime;
    public float moveSpeed;
    public float attackSpeed;
    public float attackPower;
    public float defensePower;

    public Stats(float hP, float coolTime, float moveSpeed, float attackSpeed, float attackPower, float defensePower)
    {
        HP = hP;
        this.coolTime = coolTime;
        this.moveSpeed = moveSpeed;
        this.attackSpeed = attackSpeed;
        this.attackPower = attackPower;
        this.defensePower = defensePower;
    }
}
