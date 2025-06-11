using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ZZZ.Framework.Injecting;

public class AssemblyResolver : IAssemblyResolver
{
    public IEnumerable<Assembly> Assemblies => assemblySystemCache.Values;

    private Dictionary<string, string> referenceDictionary = new();
    private Dictionary<string, AssemblyDefinition> assemblyDefinitionCache = new(StringComparer.InvariantCultureIgnoreCase);
    private Dictionary<string, Assembly> assemblySystemCache = new(StringComparer.InvariantCultureIgnoreCase);

    public AssemblyResolver(IEnumerable<string> splitReferences, IEnumerable<string> editors)
    {
        foreach (var filePath in splitReferences)
        {
            var name = GetAssemblyName(filePath);

            referenceDictionary[name] = filePath;

            if (editors.Contains(name))
                assemblySystemCache.Add(name, Assembly.LoadFrom(filePath));
        }
    }

    private string GetAssemblyName(string filePath)
    {
        try
        {
            return GetAssembly(filePath, new(ReadingMode.Deferred)).Name.Name;
        }
        catch (Exception)
        {
            return Path.GetFileNameWithoutExtension(filePath);
        }
    }

    private AssemblyDefinition GetAssembly(string file, ReaderParameters parameters)
    {
        if (assemblyDefinitionCache.TryGetValue(file, out var assembly))
        {
            return assembly;
        }

        parameters.AssemblyResolver ??= this;

        try
        {
            return assemblyDefinitionCache[file] = AssemblyDefinition.ReadAssembly(file, parameters);
        }
        catch (Exception exception)
        {
            throw new($"Could not read '{file}'.", exception);
        }
    }

    public virtual AssemblyDefinition Resolve(AssemblyNameReference assemblyNameReference) =>
        Resolve(assemblyNameReference, new());

    public virtual AssemblyDefinition Resolve(AssemblyNameReference assemblyNameReference, ReaderParameters parameters)
    {
        parameters ??= new();

        if (referenceDictionary.TryGetValue(assemblyNameReference.Name, out var fileFromDerivedReferences))
        {
            return GetAssembly(fileFromDerivedReferences, parameters);
        }

        return null;
    }

    public virtual Assembly Resolve(AssemblyName assemblyName)
    {
        if (assemblySystemCache.TryGetValue(assemblyName.Name, out var assembly))
        {
            return assembly;
        }

        return null;
    }

    public virtual void Dispose()
    {
        foreach (var value in assemblyDefinitionCache.Values)
        {
            value?.Dispose();
        }
    }
}