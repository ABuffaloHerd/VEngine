using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using VEngine.Data;
using VEngine.Events;
using VEngine.Factory;
using VEngine.Logging;
using VEngine.Objects;
using VEngine.Animations;
using System.Collections.Immutable;
using System.Transactions;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace VEngine.Items
{
    public static class SpellRegistry
    {
        public static Spell ArcaneBlast = new(
            "Arcane Blast",
            "I'm gonna play arcane blast with efficiency at level 11 so that brings the cost down to zero then in response to my declaration i am going to pay one and ignite the soul",
            30, // do the above to make your opponent think that you think you know the rules but react to the resolution of their spellshield arcane with another ignite the soul so they get one enlighten counter and take 11 damage to the face.
            20,
            new Pattern()
                .Line(7, Data.Direction.RIGHT),
            (spell, targets, wielder, arena) =>
            {
                int finalDamage = spell.Damage;
                var target = Data.Algorithms.Closest(targets, wielder.Position);
                var list = new List<GameObject>();

                AnimatedEffect effect = AnimationPresets.EnergyParticle(TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(0.05), wielder.Facing.ToVector());
                //AnimatedEffect effect = AnimationPresets.TestEffect(3, 3);
                effect.Position = wielder.Position;

                if(target != null)
                    effect.Destination = target.Position;
                
                effect.StartTimer();
                arena.PlayAnimatedEffect(effect);

                if (target != null)
                {
                    target.TakeDamage(wielder, spell, finalDamage, DamageType.MAGIC);
                    list.Add(target);
                }
                else
                    Logger.Report(null, "found no targets");

                CombatEvent ev = new CombatEventBuilder()
                    .SetEventType(CombatEventType.ATTACK)
                    .AddField("amount", finalDamage)
                    .AddField("targets", list)
                    .AddField("source", spell)
                    .AddField("weapon", new object())
                    .Build();

                return ev;
            });

        public static Spell Fireball = new(
            "Fireball",
            "Magic!",
            10,
            10,
            new Pattern()
                .Mark(2, -1)
                .Mark(3, -1)
                .Mark(4, -1)
                .Mark(2, 0)
                .Mark(3, 0)
                .Mark(4, 0)
                .Mark(2, 1)
                .Mark(3, 1)
                .Mark(4, 1),
            (spell, targets, wielder, arena) =>
            {
                int finalDamage = spell.Damage;

                foreach(GameObject obj in targets)
                {
                    obj.TakeDamage(wielder, spell, finalDamage, DamageType.MAGIC);
                }

                AnimatedEffect explode = AnimationPresets.ExplosionEffect(3, TimeSpan.FromSeconds(0.2));

                explode.Position = wielder.Facing switch
                {
                    Data.Direction.UP => (0, -3),
                    Data.Direction.DOWN => (0, 3),
                    Data.Direction.LEFT => (-3, 0),
                    Data.Direction.RIGHT => (3, 0),
                    _ => throw new Exception("Switch fucked up.")
                }
                + wielder.Position;

                arena.PlayAnimatedEffect(explode);

                CombatEvent ev = new CombatEventBuilder()
                    .SetEventType(CombatEventType.ATTACK)
                    .AddField("amount", finalDamage)
                    .AddField("targets", targets)
                    .AddField("source", spell)
                    .AddField("weapon", new object())
                    .Build();

                return ev;
            });


        public static Spell Lightning = new(
            "Lightning",
            "Strike the 3 healthiest targets in range with shocking electricity. Deals 50% of target's max HP.",
            30,
            30,
            new Pattern()
                .Mark(2, -1)
                .Mark(3, -1)
                .Mark(4, -1)
                .Mark(2, 0)
                .Mark(3, 0)
                .Mark(4, 0)
                .Mark(2, 1)
                .Mark(3, 1)
                .Mark(4, 1),
            (spell, targets, wielder, arena) =>
            {
                int total = 0;
                // sort targets

                List<GameObject> sorted = targets.ToList();
                sorted.Sort(new HPComparer());
                sorted.Reverse();

                int counter = 0;
                for(int x = 0;  x < sorted.Count() && x < 3; x++)
                {
                    GameObject current = sorted[x];
                    int amount = (int)Math.Floor(current.HP.Current * 0.5);
                    int taken = sorted[x].TakeDamage(wielder, spell, amount, DamageType.MAGIC);

                    total += taken;
                }

                AnimatedEffect lightning = AnimationPresets.Lightning(3, TimeSpan.FromSeconds(4));

                lightning.Position = wielder.Facing switch
                {
                    Data.Direction.UP => (0, -3),
                    Data.Direction.DOWN => (0, 3),
                    Data.Direction.LEFT => (-3, 0),
                    Data.Direction.RIGHT => (3, 0),
                    _ => throw new Exception("Switch fucked up.")
                }
                + wielder.Position;
                arena.PlayAnimatedEffect(lightning);

                CombatEvent ev = new CombatEventBuilder()
                    .SetEventType(CombatEventType.ATTACK)
                    .AddField("amount", total)
                    .AddField("targets", targets)
                    .AddField("source", spell)
                    .AddField("weapon", new object())
                    .Build();

                return ev;
            });

    }

    class HPComparer : Comparer<GameObject>
    {
        public override int Compare(GameObject? x, GameObject? y)
        {
            if (x == null || y == null) return 0;
            return x.HP.Current - y.HP.Current;
        }
    }
}
