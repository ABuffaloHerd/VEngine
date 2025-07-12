using System;
using System.Collections.Generic;
using VEngine.Scenes.Combat;

namespace VEngine.Objects
{
    public class MagicCircle : StaticGameObject
    {
        private GameObject owner;

        private static AnimatedScreenObject prepareAppearance(Color color)
        {
            AnimatedScreenObject aso = new("circle", 1, 1);
            aso.CreateFrame()[0].Foreground = color;
            aso.Frames[0].SetGlyph(0, 0, '@');

            return aso;
        }

        public MagicCircle(Color color, Alignment alignment, GameObject owner) : base(prepareAppearance(color), -1)
        {
            Type = EntityType.CIRCLE;
            Alignment = alignment;
            this.owner = owner;
        }

        /// <summary>
        /// Get the owner of this magic circle
        /// </summary>
        public GameObject Owner => owner;

        /// <summary>
        /// Check if this circle belongs to a specific mage
        /// </summary>
        public bool IsOwnedBy(GameObject mage)
        {
            return owner == mage;
        }

        /// <summary>
        /// Get the number of magic circles owned by a specific mage
        /// </summary>
        public static int CountCirclesOwnedBy(GameObject mage, Arena arena)
        {
            int count = 0;
            foreach (var entity in arena.EntityManager)
            {
                if (entity is MagicCircle circle && circle.IsOwnedBy(mage))
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// Get the number of magic circles owned by a specific mage using arena context
        /// </summary>
        public static int CountCirclesOwnedBy(GameObject mage)
        {
            if (mage.HasArena)
            {
                return CountCirclesOwnedBy(mage, mage.Arena!);
            }
            return 0;
        }

        public override string ToString()
        {
            return $"Magic Circle (Owner: {owner?.Name ?? "Unknown"})";
        }
    }
}
