using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace ZZZ.Framework.Injecting;

public class InjectingTask : Task
{
    [Required]
    public string AssemblyFile { get; set; }

    public bool IsDebug { get; set; }

    [Required]
    public string References { get; set; } = null!;

    [Required]
    [Description("Пути до файлов сборок редакторов кода, указанные через ';'.")]
    public string Editors { get; set; } = null!;


    public override bool Execute()
    {
        if (IsDebug)
            Debugger.Launch();

        try
        {
            var references = References
            .Split([';'], StringSplitOptions.RemoveEmptyEntries)
            .ToList();


            var editors = Editors
            .Split([';'], StringSplitOptions.RemoveEmptyEntries)
            .ToList();

            Injector injector = new Injector(AssemblyFile, references, editors);

            injector.ProcessInjecting();

            return true;
        }
        catch (Exception exception)
        {
            Log.LogErrorFromException(exception);

            return false;
        }
    }
}