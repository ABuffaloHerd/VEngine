namespace VEngine.Events
{
    public class SceneChangeEvent : GameEvent
    {
        public string? TargetScene;
        public override EventTarget Target => EventTarget.SCENE_MANAGER;

        /// <summary>
        /// Use this one instead
        /// </summary>
        /// <param name="targetScene">Name of target scene</param>
        public SceneChangeEvent(string targetScene) : base()
        {
            TargetScene = targetScene;
            AddData("change_scene", targetScene);
        }

        /// <summary>
        /// Exists for compatibility and event pool usage
        /// </summary>
        public SceneChangeEvent() { }

        /// <summary>
        /// Initialize the event with target scene data
        /// </summary>
        /// <param name="targetScene">Name of target scene</param>
        /// <returns>This event for chaining</returns>
        public SceneChangeEvent Initialize(string targetScene)
        {
            TargetScene = targetScene;
            AddData("change_scene", targetScene);
            return this;
        }
    }
}
