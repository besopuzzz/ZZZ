using System.Reflection;

namespace ZZZ.Framework.Attributes
{
    public readonly struct AttributeInfo<TAttribute> : IEquatable<AttributeInfo<TAttribute>>
        where TAttribute : IComponentAttribute<TAttribute>
    {
        public TAttribute Attribute { get; }
        public MemberInfo MemberInfo { get; }
        public AttributeTargets AttributeTargets { get; }
        public object Tag { get; }

        public AttributeInfo(
            TAttribute attribute, MemberInfo memberInfo, AttributeTargets attributeTargets, object tag = null)
        {
            ArgumentNullException.ThrowIfNull(attribute);
            ArgumentNullException.ThrowIfNull(memberInfo);

            Attribute = attribute;
            MemberInfo = memberInfo;
            AttributeTargets = attributeTargets;
            Tag = tag;
        }

        public readonly bool Equals(AttributeInfo<TAttribute> other)
        {
            return this == other;
        }

        public static bool operator !=(AttributeInfo<TAttribute> left, AttributeInfo<TAttribute> right)
        {
            return !(left == right);
        }

        public static bool operator ==(AttributeInfo<TAttribute> left, AttributeInfo<TAttribute> right)
        {
            if (right.Tag != left.Tag && right.AttributeTargets != left.AttributeTargets && right.MemberInfo != left.MemberInfo)
                return false;

            return right.Attribute.Equals(left.Attribute);
        }

        public override readonly bool Equals(object obj)
        {
            return obj is AttributeInfo<TAttribute> info && Equals(info);
        }

        public override readonly int GetHashCode()
        {
            return (((17 * 23 + Attribute.GetHashCode()) * 23 + MemberInfo.GetHashCode())
                * 23 + AttributeTargets.GetHashCode())
                * (Tag != null ? 23 + Tag.GetHashCode() : 1);
        }
    }
}

