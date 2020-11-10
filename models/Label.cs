namespace VideoMetaInfo.models
{
    public class Label
    {
        public string Name { get; set; }

        public string Tag { get; set; }

        public Label(string name, string tag)
        {
            Name = name;
            Tag = tag;
        }
    }
}
