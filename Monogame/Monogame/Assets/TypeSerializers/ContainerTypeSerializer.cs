using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;

namespace ZZZ.Framework.Monogame.Assets.TypeSerializers
{
    [ContentTypeSerializer]
    public class ContainerTypeSerializer : ContentTypeSerializer<Container>
    {
        protected override Container Deserialize(IntermediateReader input, ContentSerializerAttribute format, Container existingInstance)
        {
            if(existingInstance == null)
                existingInstance = new Container();

            existingInstance.Name = input.ReadObject<string>(new ContentSerializerAttribute() { ElementName = "Name" });
            existingInstance.Enabled = input.ReadObject<bool>(new ContentSerializerAttribute() { ElementName = "Enabled" });
            var components = input.ReadObject<IEnumerable<IComponent>>(new ContentSerializerAttribute() { ElementName = "Components" });
            var containers = input.ReadObject<IEnumerable<IContainer>>(new ContentSerializerAttribute() { ElementName = "Containers" });

            foreach ( var component in components )
                existingInstance.AddComponent(component);

            foreach (var container in containers)
                existingInstance.AddContainer(container);

            return existingInstance;
        }

        protected override void Serialize(IntermediateWriter output, Container value, ContentSerializerAttribute format)
        {
            output.WriteObject(value.Name, new ContentSerializerAttribute() { ElementName = "Name"});
            output.WriteObject(value.Enabled, new ContentSerializerAttribute() { ElementName = "Enabled" });
            output.WriteObject(value.GetComponents(), new ContentSerializerAttribute() { ElementName = "Components"});
            output.WriteObject(value.GetContainers(), new ContentSerializerAttribute() { ElementName = "Containers" });
        }
    }
}
