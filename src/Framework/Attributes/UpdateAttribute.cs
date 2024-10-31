
using System.Reflection;

namespace ZZZ.Framework.Attributes
{
    public class UpdateAttribute : MethodComponentAttribute<UpdateAttribute>
    {


        public UpdateAttribute()
        {

        }

        public void Update(object obj, GameTime gameTime)
        {
            Invoke(obj, gameTime);
        }
    }

    public class UpdateAttributeFactory : AttributeFactory<UpdateAttribute>
    {
        protected override IEnumerable<AttributeInfo<UpdateAttribute>> GetMembers<T>(AttributeTargets attributeTargets)
        {
            var target = typeof(T);

            var attributeCache = new List<AttributeInfo<UpdateAttribute>>();

            var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            var methods = target.GetMethods(flags);

            foreach (var method in methods)
                foreach (var attribute in method.GetCustomAttributes<UpdateAttribute>())
                    attributeCache.Add(new AttributeInfo<UpdateAttribute>(attribute, method, AttributeTargets.Method));

            return attributeCache;
        }
    }
}
