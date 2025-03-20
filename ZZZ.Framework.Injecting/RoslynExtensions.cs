using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ZZZ.Framework.Injecting
{
    public static class RoslynExtensions
    {
        public static IEnumerable<AttributeSyntax> FindAttributes(this MethodDeclarationSyntax method, SemanticModel semanticModel)
        {
            var attributes = method.AttributeLists.SelectMany(al => al.Attributes);

            var fined = new List<AttributeSyntax>();

            foreach (var attribute in attributes)
            {
                var typeInfo = semanticModel.GetTypeInfo(attribute).ConvertedType;

                if (typeInfo.Name == "EventListener")
                    fined.Add(attribute);
            }

            return fined;
        }
    }
}
