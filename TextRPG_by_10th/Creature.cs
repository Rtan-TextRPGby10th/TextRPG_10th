﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextRPG_by_10th
{
    public class Creature
    {
        public string Name { get; set; } // 이름
        public float Health { get; set; } // 현재 체력
        public float MaxHealth { get; set; } // 최대 체력
        public float AttackPower { get; set; } // 공격력
        public float Defense { get; set; } // 방어력
        public int Lv { get; set; } // 레벨
        public float HitChance { get; set; } // 명중률 (0~1)
        public float DodgeChance { get; set; } // 회피율 (0~1)

        public float CritChance { get; set; } // 치명타율 (0~1)

        public bool isDie = false; // 사망 확인

        

        public enum CreatureType
        {
            Player,
            Monster
        }

        public CreatureType creatureType { get; set; }

        public Battle.DebuffType debuffType { get; set; } = Battle.DebuffType.NONE;
        public Creature(string name, float health, float attackPower, float defense, int lv, float hitChance = 0.8f, float dodgeChance = 0.1f, float critChance = 0.1f) // 크리처 생성자
        {
            Name = name;
            Health = health;
            AttackPower = attackPower;
            Defense = defense;
            Lv = lv;
            HitChance = hitChance;
            DodgeChance = dodgeChance;
            CritChance = critChance;
        }

        public void TakeDamage(float damage) // 몬스터나 캐릭터의 공격력을 받아와 데미지를 받는 함수
        {
            Health -= damage;

            if (Health <= 0)
            {
                Health = 0; // 체력의 최소값으로 강제 설정
                isDie = true;
                Console.WriteLine($"{Name}이(가) 사망하였습니다.");
            } 
        }

        public void Healing(float heal) // 체력을 회복하는 함수
        {
            Health += heal;
            if(isDie)
            {
                isDie = false;
                Console.WriteLine($"{Name}이(가) 부활하였습니다.");

                if (Health > MaxHealth)
                {
                    Health = MaxHealth; // 체력의 최대값으로 강제 설정
                }
            }
        }
    }
}
