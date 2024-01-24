using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VEngine.Objects;
using VEngine.Data;
using VEngine.Events;
using VEngine.Logging;
using VEngine.Factory;

using Algorithms = VEngine.Data.Algorithms;

namespace VEngine.Items
{
    public static class WeaponRegistry
    {
        public static Weapon Hands = new(
            "Bare Hands",
            "Give em' the hands! - Oskar", // Todo: Choose between this and "I COULD KILL YOU IN FIVE SECONDS WITH ME BARE HANDS. I'M TRAINED I AM" - Michael Rosen
            1, 
            new Pattern().Mark(1, 0),
            (weapon, targets, wielder, arena) =>
            {
                int finalDamage = weapon.Damage;
                foreach (GameObject obj in targets)
                {
                    obj.TakeDamage(wielder, weapon, finalDamage, DamageType.PHYSICAL);
                }

                CombatEvent ev = new CombatEventBuilder()
                    .SetEventType(CombatEventType.ATTACK)
                    .AddField("amount", finalDamage)
                    .AddField("targets", targets)
                    .AddField("source", wielder)
                    .AddField("weapon", weapon)
                    .Build();

                return ev;
            });

        public static Weapon WoodenSword = new(
            "Wooden sword",
            "A wooden stick that barely meets the criteria of being a weapon",
            3,
            new Pattern().Mark(1, 0),
            (weapon, targets, wielder, arena) =>    
            {
                int finalDamage = weapon.Damage;
                foreach(GameObject obj in targets)
                {
                    obj.TakeDamage(wielder, weapon, finalDamage, DamageType.PHYSICAL);
                }

                CombatEvent ev = new CombatEventBuilder()
                    .SetEventType(CombatEventType.ATTACK)
                    .AddField("amount", finalDamage)
                    .AddField("targets", targets)
                    .AddField("source", wielder)
                    .AddField("weapon", weapon)
                    .Build();

                return ev;
            });

        public static RangedWeapon Rifle = new(
            "Makeshift rifle",
            "A pistol with a long rifled barrel and telescope taped to it.",
            5,
            2,
            new Pattern().Mark(2, 0).Mark(3, 0).Mark(4, 0).Mark(5, 0),
            (weapon, targets, wielder, arena) =>
            {
                int finalDamage = 0;
                var target = Data.Algorithms.Closest(targets, wielder.Position);
                var list = new List<GameObject>();

                if (target != null)
                {
                    Logger.Report("rifle", $"Found a single target to attack at {target.Position.X}, {target.Position.Y}!");

                    if (Algorithms.CheckLineOfSight(target.Position, wielder.Position, target, wielder, arena))
                    {
                        finalDamage = target.TakeDamage(wielder, weapon, weapon.Damage, DamageType.PHYSICAL);
                        list.Add(target);
                    }
                    else
                    {
                        Logger.Report("rifle", "line of sight check failed");
                    }
                }
                else
                    Logger.Report(null, "found no targets");

                CombatEvent ev = new CombatEventBuilder()
                    .SetEventType(CombatEventType.ATTACK)
                    .AddField("amount", finalDamage)
                    .AddField("targets", list)
                    .AddField("source", wielder)
                    .AddField("weapon", weapon)
                    .Build();

                return ev;
            });

        public static Weapon CombatSword = new(
            "Federation Issue Sword",
            "Standard issue shortsword. Deals 4 + 50% DEF damage twice.",
            4,
            new Pattern().Mark(1, 0),
            (weapon, targets, wielder, arena) =>
            {
                int finalDamage = 0;
                var target = targets.FirstOrDefault();

                if (target != null) 
                {
                    int calcDamage = (wielder.DEF / 2).Current + weapon.Damage;
                    finalDamage += target.TakeDamage(wielder, weapon, calcDamage, DamageType.PHYSICAL);
                    finalDamage += target.TakeDamage(wielder, weapon, calcDamage, DamageType.PHYSICAL);
                }

                CombatEvent ev = new CombatEventBuilder()
                    .SetEventType(CombatEventType.ATTACK)
                    .AddField("amount", finalDamage)
                    .AddField("targets", targets)
                    .AddField("source", wielder)
                    .AddField("weapon", weapon)
                    .Build();

                return ev;
            });

        public static Sword ChiXiao = new(
            "Chi Xiao",
            "Deals damage twice.\nAbility: Pick up to 10 targets and slash all of them, dealing 140% damage. Lucky last gets 300% physical and magic damage.",
            5,
            new Pattern().Mark(1, 0),
            (weapon, targets, wielder, arena) =>
            {
                int finalDamage = 0;
                var target = targets.FirstOrDefault();

                if (target != null)
                {
                    int calcDamage = weapon.Damage;
                    finalDamage += target.TakeDamage(wielder, weapon, calcDamage, DamageType.PHYSICAL);
                    finalDamage += target.TakeDamage(wielder, weapon, calcDamage, DamageType.PHYSICAL);
                }

                CombatEvent ev = new CombatEventBuilder()
                    .SetEventType(CombatEventType.ATTACK)
                    .AddField("amount", finalDamage)
                    .AddField("targets", targets)
                    .AddField("source", wielder)
                    .AddField("weapon", weapon)
                    .Build();

                return ev;
            },
            new Pattern(Pattern.Presets.Diamond(3).ToHashSet()),
            2,
            (weapon, targets, wielder, arena) =>
            {
                IEnumerator<GameObject> enumerator = targets.GetEnumerator();
                int tally = 0;
                for (int x = 0; x < 10; x++)
                {
                    if(!enumerator.MoveNext())
                    {
                        enumerator.Reset();
                        enumerator.MoveNext();
                    }

                    tally += enumerator.Current.TakeDamage(wielder, weapon, (int)(weapon.Damage * 1.4f), DamageType.PHYSICAL);
                    if(x == 10)
                    {
                        tally += enumerator.Current.TakeDamage(wielder, weapon, weapon.Damage * 3, DamageType.PHYSICAL);
                        tally += enumerator.Current.TakeDamage(wielder, weapon, weapon.Damage * 3, DamageType.MAGIC);
                    }
                }

                CombatEvent ev = new CombatEventBuilder()
                    .SetEventType(CombatEventType.ATTACK)
                    .AddField("amount", tally)
                    .AddField("targets", targets)
                    .AddField("source", wielder)
                    .AddField("weapon", weapon)
                    .Build();

                return ev;
            });

        public static Weapon Wrench = new(
            "Monkey Wrench",
            "Fixes things and bludgeons",
            5,
            new Pattern().Mark(1, 0),
            (weapon, targets, wielder, arena) =>
            {
                int finalDamage = weapon.Damage;
                foreach (GameObject obj in targets)
                {
                    obj.TakeDamage(wielder, weapon, finalDamage, DamageType.PHYSICAL);
                }

                CombatEvent ev = new CombatEventBuilder()
                    .SetEventType(CombatEventType.ATTACK)
                    .AddField("amount", finalDamage)
                    .AddField("targets", targets)
                    .AddField("source", wielder)
                    .AddField("weapon", weapon)
                    .Build();

                return ev;
            });
    }
}
