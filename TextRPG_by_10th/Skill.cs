using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG_by_10th
{
    public enum SkillType
    {
        SkillAttack,
        Buff
    }
    public class Skill
    {
        public string Name { get; set; }
        public SkillType Type { get; set; }
        public float Power { get; set; }
        public int Cooldown { get; set; }

        public Skill(string skillName, SkillType type, float power, int cooldown) 
        {
            Name = skillName;
            Type = type;
            Power = power;
            Cooldown = cooldown;
        }

        public float CalculatePower(Player player)
        {
            if (Type == SkillType.SkillAttack)
            {
                return player.AttackPower * Power;
            }
            return Power;
        }
    }
}
