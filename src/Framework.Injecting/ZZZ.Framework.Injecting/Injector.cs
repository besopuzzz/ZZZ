using Mono.Cecil;
using Mono.Cecil.Pdb;
using Mono.Cecil.Rocks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ZZZ.Framework.Injecting
{
    public sealed class Injector
    {
        private AssemblyResolver assemblyResolver;
        private string assemblyFileName;
        private ResolveEventHandler resolveEventHandler;
        private Editors substances;

        public Injector(string assemblyFileName, IEnumerable<string> references, IEnumerable<string> editors)
        {
            if (string.IsNullOrEmpty(assemblyFileName))
                throw new ArgumentNullException(nameof(assemblyFileName));

            this.assemblyFileName = assemblyFileName;

            assemblyResolver = new AssemblyResolver(references, editors);

            substances = new Editors(assemblyResolver.Assemblies);
        }

        public void ProcessInjecting()
        {
            resolveEventHandler = CurrentDomain_AssemblyResolve;

            AppDomain.CurrentDomain.AssemblyResolve += resolveEventHandler;

            var readerParameters = new ReaderParameters
            {
                AssemblyResolver = assemblyResolver,
                InMemory = true
            };

            var module = ModuleDefinition.ReadModule(assemblyFileName, readerParameters);

            var hasSymbols = false;
            try
            {
                module.ReadSymbols();
                hasSymbols = true;
            }
            catch
            {
            }


            foreach (var substance in substances.Substances)
            {
                try
                {
                    substance.Inject(module);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{substance.GetType().FullName} inject error: {ex}");
                }
            }



            var parameters = new WriterParameters
            {
                WriteSymbols = hasSymbols
            };

            module.Write(assemblyFileName, parameters);

            module.Dispose();

            AppDomain.CurrentDomain.AssemblyResolve -= resolveEventHandler;
        }



        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var assemblyName = new AssemblyName(args.Name).Name;

            if (assemblyName == "Mono.Cecil")
            {
                return typeof(ModuleDefinition).Assembly;
            }

            if (assemblyName == "Mono.Cecil.Rocks")
            {
                return typeof(MethodBodyRocks).Assembly;
            }

            if (assemblyName == "Mono.Cecil.Pdb")
            {
                return typeof(PdbReaderProvider).Assembly;
            }

            return null;
        }
    }

}
