using VEngine.Items;

namespace VEngine.Objects.Classes
{
    // High movement to open up the map
    public class Vanguard : ControllableGameObject
    {
        public Vanguard(AnimatedScreenObject appearance, int zIndex) : base(appearance, zIndex)
        {
            MoveDist = 20;
            Weapon = (Weapon)WeaponRegistry.CombatSword.Clone();
        }
    }
}
