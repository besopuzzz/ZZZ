using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Xml.Serialization;
using ZZZ.AttributeIntegrations;

namespace ZZZ.AttributeAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ZZZAttributeAnalyzerAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "ZZZAttributeAnalizer";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Usage";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        [MethodAttribute<AnalysisContext>] 
        [MethodAttribute<AnalysisContext>] 
        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeSymbol, SyntaxKind.MethodDeclaration);
        }
         
        private static void AnalyzeSymbol(SyntaxNodeAnalysisContext context)
        {
            var methodDeclaration = (MethodDeclarationSyntax)context.Node;

            foreach (var item in methodDeclaration.AttributeLists)
            {
                foreach (var attribute in item.Attributes)
                {
                    var info = context.SemanticModel.GetTypeInfo(attribute, context.CancellationToken);

                    //if (info.Type.AllInterfaces.Any(x => x.MetadataName != "IParameterAttribute"))
                    //    continue;

                    context.ReportDiagnostic(Diagnostic.Create(Rule, context.Node.GetLocation(), $"Count: {attribute.Ge.ArgumentList.Arguments.Count}"));

                    foreach (var argument in attribute.ArgumentList.Arguments)
                    {

                        var argsInfo = context.SemanticModel.GetTypeInfo(argument, context.CancellationToken);

                        context.ReportDiagnostic(Diagnostic.Create(Rule, context.Node.GetLocation(), argsInfo.Type.MetadataName));
                    }
                }
             }
        }
    }
}
