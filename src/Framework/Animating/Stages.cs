using Microsoft.Xna.Framework.Content;
using System.Collections;
using ZZZ.Framework;

namespace ZZZ.Framework.Animations
{
    public sealed class Stages
    {
        public Stage this[string name] => stages.Find(x => x.Name == name);

        [ContentSerializerIgnore]
        public int Count => stages.Count;

        [ContentSerializer(ElementName = "Stages")]
        private SharedList<Stage> stages = new SharedList<Stage>();

        internal Stages()
        {

        }

        public void Add(Stage stage)
        {
            if (Contains(stage.Name))
                throw new Exception($"Stage with name {stage.Name} already exist! Create unique name for stage!");

            stages.Add(stage);
        }
        public bool Remove(Stage stage) => stages.Remove(stage);
        public bool Remove(string name) => Remove(this[name]);
        public bool Contains(string name) => this[name] != null;
        public bool Contains(Stage stage) => stages.Contains(stage);
        public void Clear() => stages.Clear();
        internal Stage First() => stages.FirstOrDefault();
    }
}
