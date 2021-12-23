﻿using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Unity.CompilationPipeline.Common.Diagnostics;
using Unity.CompilationPipeline.Common.ILPostProcessing;

namespace Hertzole.ALE.CodeGen
{
	public sealed class LevelEditorILProcessor : ILPostProcessor
	{
		private readonly List<DiagnosticMessage> diagnostics = new List<DiagnosticMessage>();
		private Weaver weaver;
		private const string RUNTIME_ASSEMBLY = "Hertzole.ALE";

		public override ILPostProcessor GetInstance()
		{
			return this;
		}

		public override bool WillProcess(ICompiledAssembly compiledAssembly)
		{
			for (int i = 0; i < compiledAssembly.References.Length; i++)
			{
				if (Path.GetFileNameWithoutExtension(compiledAssembly.References[i]) == RUNTIME_ASSEMBLY)
				{
					return true;
				}
			}

			return false;
		}

		public override ILPostProcessResult Process(ICompiledAssembly compiledAssembly)
		{
			if (!WillProcess(compiledAssembly))
			{
				return null;
			}

#if ALE_DEBUG
			Stopwatch sw = Stopwatch.StartNew();
#endif

			diagnostics.Clear();

			weaver ??= new Weaver(diagnostics);

			AssemblyDefinition assemblyDefinition = AssemblyDefinitionFor(compiledAssembly);
			if (assemblyDefinition == null)
			{
				diagnostics.AddError($"Cannot read assembly definition: {compiledAssembly.Name}");
				return null;
			}

			ModuleDefinition mainModule = assemblyDefinition.MainModule;
			if (mainModule != null)
			{
				weaver.ProcessAssembly(mainModule, compiledAssembly.Defines);
			}
			else
			{
				diagnostics.AddError($"Cannot get main module from assembly definition: {compiledAssembly.Name}");
				return null;
			}

			MemoryStream pe = new MemoryStream();
			MemoryStream pdb = new MemoryStream();

			WriterParameters writerParameters = new WriterParameters
			{
				SymbolWriterProvider = new PortablePdbWriterProvider(),
				SymbolStream = pdb,
				WriteSymbols = true
			};

			assemblyDefinition.Write(pe, writerParameters);

#if ALE_DEBUG
			sw.Stop();
			diagnostics.AddWarning($"ALE ILPostProcessor took {sw.ElapsedMilliseconds}ms ({sw.ElapsedTicks} ticks) when processing {compiledAssembly.Name}.");
#endif

			return new ILPostProcessResult(new InMemoryAssembly(pe.ToArray(), pdb.ToArray()), diagnostics);
		}

		public static AssemblyDefinition AssemblyDefinitionFor(ICompiledAssembly compiledAssembly)
		{
			PostProcessorAssemblyResolver assemblyResolver = new PostProcessorAssemblyResolver(compiledAssembly);
			ReaderParameters readerParameters = new ReaderParameters
			{
				SymbolStream = new MemoryStream(compiledAssembly.InMemoryAssembly.PdbData),
				SymbolReaderProvider = new PortablePdbReaderProvider(),
				AssemblyResolver = assemblyResolver,
				ReflectionImporterProvider = new PostProcessorReflectionImporterProvider(),
				ReadingMode = ReadingMode.Immediate,
				ReadSymbols = true
			};

			AssemblyDefinition assemblyDefinition = AssemblyDefinition.ReadAssembly(new MemoryStream(compiledAssembly.InMemoryAssembly.PeData), readerParameters);

			// Apparently, it will happen that when we ask to resolve a type that lives inside Hertzole.ALE, and we
			// are also postprocessing Hertzole.ALE, type resolving will fail, because we do not actually try to resolve
			// inside the assembly we are processing. Let's make sure we do that, so that we can use postprocessor features inside
			// Hertzole.ALE itself as well.
			assemblyResolver.AddAssemblyDefinitionBeingOperatedOn(assemblyDefinition);

			return assemblyDefinition;
		}
	}
}