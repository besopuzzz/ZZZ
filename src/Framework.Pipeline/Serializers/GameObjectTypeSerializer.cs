using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using System.Collections.Generic;

namespace ZZZ.KNI.Content.Pipeline.Serializers
{
    [ContentTypeSerializer]
    public class GameObjectTypeSerializer : ContentTypeSerializer<GameObject>
    {
        protected override GameObject Deserialize(IntermediateReader input, ContentSerializerAttribute format, GameObject existingInstance)
        {
            if(existingInstance == null)
                existingInstance = Activator.CreateInstance(TargetType, true) as GameObject;

            existingInstance.Name = input.ReadObject<string>(new ContentSerializerAttribute() { ElementName = "Name" });
            existingInstance.Enabled = input.ReadObject<bool>(new ContentSerializerAttribute() { ElementName = "Enabled" });
            var components = input.ReadObject<IEnumerable<IComponent>>(new ContentSerializerAttribute() { ElementName = "GameComponents" });
            var containers = input.ReadObject<IEnumerable<GameObject>>(new ContentSerializerAttribute() { ElementName = "GameObjects" });

            foreach ( var component in components )
                existingInstance.AddComponent(component);

            foreach (var container in containers)
                existingInstance.AddGameObject(container);

            return existingInstance;
        }

        protected override void Serialize(IntermediateWriter output, GameObject value, ContentSerializerAttribute format)
        {
            output.WriteObject(value.Name, new ContentSerializerAttribute() { ElementName = "Name"});
            output.WriteObject(value.Enabled, new ContentSerializerAttribute() { ElementName = "Enabled" });
            output.WriteObject(value.GetComponents(), new ContentSerializerAttribute() { ElementName = "GameComponents"});
            output.WriteObject(value.GetGameObjects(), new ContentSerializerAttribute() { ElementName = "GameObjects" });
        }
    }
}
