using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG_by_10th
{
    internal class Creature
    {
        public string Name { get; set; }
        public float Health { get; set; }
        public float AttackPower { get; set; }
        public float Defense { get; set; }
        public int Lv { get; set; }

        public Creature(string name, float health, float attackPower, float defense, int lv) 
        {
            Name = name;
            Health = health;
            AttackPower = attackPower;
            Defense = defense;
            Lv = lv;
        }

        public void TakeDamage(float damage)
        {
            Health -= damage;
            if (Health < 0)
            {
                Health = 0;
            } 
        }

        public void Healing(float heal)
        {
            Health += heal;
            if (Health > 100)
            {
                Health = 100;
            }
        }
    }
}
