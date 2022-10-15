namespace BWolf.SceneSearch
{
    public readonly partial struct ScenePath
    {
        private readonly string _scene;

        private readonly string _root;

        public ScenePath(string wholePath)
        {
            string[] components = wholePath.Split('/');
            _scene = components[0];
            _root = components[1];
        }
    }
}
