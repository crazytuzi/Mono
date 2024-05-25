using System.Collections.Generic;
using System.IO;
using UnrealBuildTool;

public class Mono : ModuleRules
{
	public Mono(ReadOnlyTargetRules Target) : base(Target)
	{
		Type = ModuleType.External;

		PublicDefinitions.AddRange(new string[]
		{
			"DOTNET_MAJOR_VERSION=8",
			"DOTNET_MINOR_VERSION=0",
			"DOTNET_PATCH_VERSION=5"
		});

		var bIsDebug = Target.Configuration == UnrealTargetConfiguration.Debug ||
		               Target.Configuration == UnrealTargetConfiguration.DebugGame;

		var MonoConfiguration = bIsDebug ? "Debug" : "Release";

		PublicDefinitions.Add($"MONO_CONFIGURATION=TEXT(\"{MonoConfiguration}\")");

		PublicIncludePaths.Add(Path.Combine(ModuleDirectory, "src"));

		var LibraryPath = Path.Combine(ModuleDirectory, "lib", MonoConfiguration);

		if (Target.Platform == UnrealTargetPlatform.Win64)
		{
			var PlatformLibraryPath = Path.Combine(LibraryPath, Target.Platform.ToString());

			PublicAdditionalLibraries.Add(Path.Combine(PlatformLibraryPath,
				"coreclr.import.lib"));

			PublicAdditionalLibraries.Add(Path.Combine(PlatformLibraryPath,
				"mono-component-debugger-static.lib"));

			PublicAdditionalLibraries.Add(Path.Combine(PlatformLibraryPath,
				"mono-component-diagnostics_tracing-static.lib"));

			PublicAdditionalLibraries.Add(Path.Combine(PlatformLibraryPath,
				"mono-component-hot_reload-static.lib"));

			PublicAdditionalLibraries.Add(Path.Combine(PlatformLibraryPath,
				"mono-component-marshal-ilgen-static.lib"));

			PublicAdditionalLibraries.Add(Path.Combine(PlatformLibraryPath,
				"mono-profiler-aot.lib"));

			PublicAdditionalLibraries.Add(Path.Combine(PlatformLibraryPath,
				"monosgen-2.0.lib"));

			if (!Target.bBuildEditor && bIsDebug)
			{
				PublicSystemLibraries.Add("msvcrtd.lib");
			}

			RuntimeDependencies.Add("$(TargetOutputDir)/coreclr.dll",
				Path.Combine(PlatformLibraryPath, "coreclr.dll"));

			var Files = GetFiles(Path.Combine(PlatformLibraryPath, "net"));

			foreach (var File in Files)
			{
				var ModuleLastDirectory = Path.GetFullPath(Path.Combine(ModuleDirectory, ".."));

				var DestPath = File.Substring(ModuleLastDirectory.Length + 1,
					File.Length - ModuleLastDirectory.Length - 1);

				RuntimeDependencies.Add("$(BinaryOutputDir)/" + DestPath, File);
			}
		}
		else if (Target.Platform == UnrealTargetPlatform.Android)
		{
			var PlatformLibraryPath = Path.Combine(LibraryPath, Target.Platform.ToString());

			PublicAdditionalLibraries.Add(Path.Combine(PlatformLibraryPath,
				"libmonosgen-2.0.a"));

			var Files = GetFiles(Path.Combine(PlatformLibraryPath, "net"));

			foreach (var File in Files)
			{
				var ModuleLastDirectory = Path.GetFullPath(Path.Combine(ModuleDirectory, ".."));

				var DestPath = File.Substring(ModuleLastDirectory.Length + 1,
					File.Length - ModuleLastDirectory.Length - 1);

				RuntimeDependencies.Add("$(BinaryOutputDir)/" + DestPath, File);
			}

			var APLName = "Mono_APL.xml";

			var RelativeAPLPath = Utils.MakePathRelativeTo(Path.Combine(LibraryPath, "Android"),
				Target.RelativeEnginePath);

			AdditionalPropertiesForReceipt.Add("AndroidPlugin", Path.Combine(RelativeAPLPath, APLName));
		}
		else if (Target.Platform == UnrealTargetPlatform.Linux || Target.Platform == UnrealTargetPlatform.LinuxArm64)
		{
			var PlatformLibraryPath = Path.Combine(LibraryPath,
				Target.Platform == UnrealTargetPlatform.Linux ? "Linux_x86_64" : "Linux_aarch64");

			PublicAdditionalLibraries.Add(Path.Combine(PlatformLibraryPath,
				"libmonosgen-2.0.a"));

			PublicAdditionalLibraries.Add(Path.Combine(PlatformLibraryPath,
				"libmono-component-debugger-static.a"));

			PublicAdditionalLibraries.Add(Path.Combine(PlatformLibraryPath,
				"libmono-component-diagnostics_tracing-static.a"));

			PublicAdditionalLibraries.Add(Path.Combine(PlatformLibraryPath,
				"libmono-component-hot_reload-static.a"));

			PublicAdditionalLibraries.Add(Path.Combine(PlatformLibraryPath,
				"libmono-component-marshal-ilgen-static.a"));

			PublicAdditionalLibraries.Add(Path.Combine(PlatformLibraryPath,
				"libSystem.Globalization.Native.a"));

			RuntimeDependencies.Add("$(BinaryOutputDir)/libcoreclr.so",
				Path.Combine(PlatformLibraryPath, "libcoreclr.so"));

			RuntimeDependencies.Add("$(BinaryOutputDir)/libSystem.Native.so",
				Path.Combine(PlatformLibraryPath, "libSystem.Native.so"));

			var Files = GetFiles(Path.Combine(PlatformLibraryPath, "net"));

			foreach (var File in Files)
			{
				var ModuleLastDirectory = Path.GetFullPath(Path.Combine(ModuleDirectory, ".."));

				var DestPath = File.Substring(ModuleLastDirectory.Length + 1,
					File.Length - ModuleLastDirectory.Length - 1);

				RuntimeDependencies.Add("$(BinaryOutputDir)/" + DestPath, File);
			}
		}
		else if (Target.Platform == UnrealTargetPlatform.Mac)
		{
			var PlatformLibraryPath = Path.Combine(LibraryPath, $"macOS_{Target.Architecture}");

			PublicAdditionalLibraries.Add(Path.Combine(PlatformLibraryPath,
				"libmonosgen-2.0.a"));

			PublicAdditionalLibraries.Add(Path.Combine(PlatformLibraryPath,
				"libmono-component-debugger-static.a"));

			PublicAdditionalLibraries.Add(Path.Combine(PlatformLibraryPath,
				"libmono-component-diagnostics_tracing-static.a"));

			PublicAdditionalLibraries.Add(Path.Combine(PlatformLibraryPath,
				"libmono-component-hot_reload-static.a"));

			PublicAdditionalLibraries.Add(Path.Combine(PlatformLibraryPath,
				"libmono-component-marshal-ilgen-static.a"));

			PublicAdditionalLibraries.Add(Path.Combine(PlatformLibraryPath,
				"libSystem.Globalization.Native.a"));

			RuntimeDependencies.Add("$(BinaryOutputDir)/libcoreclr.dylib",
				Path.Combine(PlatformLibraryPath, "libcoreclr.dylib"));

			RuntimeDependencies.Add("$(BinaryOutputDir)/libSystem.Native.dylib",
				Path.Combine(PlatformLibraryPath, "libSystem.Native.dylib"));

			RuntimeDependencies.Add("$(BinaryOutputDir)/libSystem.IO.Compression.Native.dylib",
				Path.Combine(PlatformLibraryPath, "libSystem.IO.Compression.Native.dylib"));

			RuntimeDependencies.Add("$(BinaryOutputDir)/libSystem.Security.Cryptography.Native.Apple.dylib",
				Path.Combine(PlatformLibraryPath, "libSystem.Security.Cryptography.Native.Apple.dylib"));

			RuntimeDependencies.Add("$(BinaryOutputDir)/libSystem.Security.Cryptography.Native.OpenSsl.dylib",
				Path.Combine(PlatformLibraryPath, "libSystem.Security.Cryptography.Native.OpenSsl.dylib"));

			RuntimeDependencies.Add("$(BinaryOutputDir)/libSystem.Net.Security.Native.dylib",
				Path.Combine(PlatformLibraryPath, "libSystem.Net.Security.Native.dylib"));

			RuntimeDependencies.Add("$(BinaryOutputDir)/libSystem.Globalization.Native.dylib",
				Path.Combine(PlatformLibraryPath, "libSystem.Globalization.Native.dylib"));

			RuntimeDependencies.Add("$(BinaryOutputDir)/libSystem.IO.Ports.Native.dylib",
				Path.Combine(PlatformLibraryPath, "libSystem.IO.Ports.Native.dylib"));

			var Files = GetFiles(Path.Combine(PlatformLibraryPath, "net"));

			foreach (var File in Files)
			{
				var ModuleLastDirectory = Path.GetFullPath(Path.Combine(ModuleDirectory, ".."));

				var DestPath = File.Substring(ModuleLastDirectory.Length + 1,
					File.Length - ModuleLastDirectory.Length - 1);

				RuntimeDependencies.Add("$(BinaryOutputDir)/" + DestPath, File);
			}
		}
		else if (Target.Platform == UnrealTargetPlatform.IOS)
		{
			var PlatformLibraryPath = Path.Combine(LibraryPath, Target.Platform.ToString());

			PublicAdditionalLibraries.Add(Path.Combine(PlatformLibraryPath,
				"System.Private.CoreLib.dll.a"));

			PublicAdditionalLibraries.Add(Path.Combine(PlatformLibraryPath,
				"libmonosgen-2.0.a"));

			PublicAdditionalLibraries.Add(Path.Combine(PlatformLibraryPath,
				"libmono-component-debugger-static.a"));

			PublicAdditionalLibraries.Add(Path.Combine(PlatformLibraryPath,
				"libmono-component-diagnostics_tracing-static.a"));

			PublicAdditionalLibraries.Add(Path.Combine(PlatformLibraryPath,
				"libmono-component-hot_reload-static.a"));

			PublicAdditionalLibraries.Add(Path.Combine(PlatformLibraryPath,
				"libmono-component-marshal-ilgen-static.a"));

			PublicAdditionalLibraries.Add(Path.Combine(PlatformLibraryPath,
				"libSystem.Globalization.Native.a"));

			PublicAdditionalLibraries.Add(Path.Combine(PlatformLibraryPath,
				"libicudata.a"));

			PublicAdditionalLibraries.Add(Path.Combine(PlatformLibraryPath,
				"libicui18n.a"));

			PublicAdditionalLibraries.Add(Path.Combine(PlatformLibraryPath,
				"libicuuc.a"));

			PublicAdditionalFrameworks.Add(
				new Framework(
					"Mono",
					Path.Combine(PlatformLibraryPath, "Mono.embeddendframework.zip"),
					null,
					true
				)
			);

			var Files = GetFiles(Path.Combine(PlatformLibraryPath, "net"));

			foreach (var File in Files)
			{
				var ModuleLastDirectory = Path.GetFullPath(Path.Combine(ModuleDirectory, ".."));

				var DestPath = File.Substring(ModuleLastDirectory.Length + 1,
					File.Length - ModuleLastDirectory.Length - 1);

				RuntimeDependencies.Add("$(BinaryOutputDir)/" + DestPath, File);
			}
		}
	}

	private static IEnumerable<string> GetFiles(string InDirectory, string InPattern = "*.*")
	{
		var Files = new List<string>();

		foreach (var File in Directory.GetFiles(InDirectory, InPattern))
		{
			Files.Add(File);
		}

		foreach (var File in Directory.GetDirectories(InDirectory))
		{
			Files.AddRange(GetFiles(File, InPattern));
		}

		return Files;
	}
}