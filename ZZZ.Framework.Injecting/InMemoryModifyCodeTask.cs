using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics;

namespace ZZZ.Framework.Injecting
{
    public class InMemoryModifyCodeTask : Microsoft.Build.Utilities.Task
    {
        [Required]
        public ITaskItem[] CompileItems { get; set; }

        [Output]
        public ITaskItem[] ModifiedCompileItems { get; set; }

        public override bool Execute()
        {
            //Debugger.Launch();

            var modifiedTrees = new List<ITaskItem>();

            foreach (var item in CompileItems)
            {
                var modifed = item;
                string filePath = item.ItemSpec;
                string sourceCode = File.ReadAllText(filePath);

                // Парсим код в синтаксическое дерево
                SyntaxTree tree = CSharpSyntaxTree.ParseText(sourceCode);
                CompilationUnitSyntax root = tree.GetRoot() as CompilationUnitSyntax;

                var compilation = CSharpCompilation.Create("TempAssembly", new[] { tree });
                compilation = compilation.AddReferences(MetadataReference.CreateFromFile(typeof(Object).Assembly.Location));


                SemanticModel semanticModel = compilation.GetSemanticModel(tree);

                // Находим и изменяем методы
                var methods = root.DescendantNodes().OfType<MethodDeclarationSyntax>();

                if (methods == null)
                    return false;



                foreach (var method in methods)
                {
                    var attributes = method.FindAttributes(semanticModel);

                    if (!attributes.Any())
                        continue;

                    var statement = new List<StatementSyntax>();
                    statement.Add(SyntaxFactory.ParseStatement("\nConsole.WriteLine(\"Call before method\");"));
                    statement.AddRange(method.Body.Statements);
                    statement.Add(SyntaxFactory.ParseStatement("\nConsole.WriteLine(\"Call after method\");\n"));


                    var newMethod = method.WithBody(
                        SyntaxFactory.Block(statement)
                    );

                    root = root.ReplaceNode(method, newMethod);

                    var tempFilePath = Path.GetTempFileName();
                    File.WriteAllText(tempFilePath, tree.WithRootAndOptions(root, tree.Options).ToString());

                    // Создание ITaskItem для временного файла
                    modifed = new TaskItem(tempFilePath);
                }

                modifiedTrees.Add(modifed);

                //    // Сохраняем измененное дерево в памяти
                //    modifiedTrees.Add(new TaskItem(filePath, new Dictionary<string, string>
                //{
                //    { "InMemoryTree", newTree.ToString() }
                //}));
            }

            // Передаем измененные деревья обратно в процесс компиляции
            ModifiedCompileItems = modifiedTrees.ToArray();

            return true;
        }
    }
}

