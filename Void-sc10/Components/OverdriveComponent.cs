using VEngine.Data;

namespace VEngine.Components
{
    /// <summary>
    /// Represents the overdrive ability of player characters
    /// </summary>
    public class OverdriveComponent : Component
    {
        public Stat Stat { get; set; } = new(0, 100);
        public OverdriveComponent() 
        {
            Stat.IsOverloadable = false;
        }
    }
}
