using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;
using Microsoft.VisualStudio.Utilities;
using Microsoft.VisualStudio.LanguageServer.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel.Composition;

namespace LuaExtension
{
	[ContentType("Lua")]
	[Export(typeof(ILanguageClient))]
	public class LuaLanguageClient : ILanguageClient
    {
		public async Task OnLoadedAsync()
		{
			if (StartAsync != null)
			{
				await StartAsync.InvokeAsync(this, EventArgs.Empty);
			}
		}

		public Task OnServerInitializedAsync()
		{
			return Task.CompletedTask;
		}

		public async Task<Connection> ActivateAsync(CancellationToken token)
		{
            var languageServer = Process.Start(new ProcessStartInfo
			{
				FileName = Path.Combine(Path.GetDirectoryName(GetType().Assembly.Location), "Server/bin/lua-language-server.exe"),
				Arguments = $"--configpath={RootPath}/.luarc.json --loglevel=trace",
				WorkingDirectory = RootPath,
				RedirectStandardInput = true,
				RedirectStandardOutput = true,
				UseShellExecute = false,
				CreateNoWindow = true
			});

			return new Connection(languageServer?.StandardOutput.BaseStream, languageServer?.StandardInput.BaseStream);
		}

		public Task<InitializationFailureContext> OnServerInitializeFailedAsync(ILanguageClientInitializationInfo initializationState)
		{
			return Task.FromResult(new InitializationFailureContext()
			{
				FailureMessage = initializationState.InitializationException?.ToString() ?? string.Empty,
			});
		}

		public string Name => "Lua Language Client";

		public IEnumerable<string> ConfigurationSections => null;

		public object InitializationOptions => new
		{
			changeConfiguration = true,
			rootPath = RootPath,
			rootUri = $"file:/// {RootPath}"
		};

		public IEnumerable<string> FilesToWatch => null;

		public bool ShowNotificationOnInitializeFailed => true;

		public event AsyncEventHandler<EventArgs> StartAsync;
		public event AsyncEventHandler<EventArgs> StopAsync;

		public string RootPath
		{
			get
			{
				var dTE = ServiceProvider.GlobalProvider.GetService(typeof(EnvDTE.DTE)) as EnvDTE.DTE;
				var directory = Path.GetDirectoryName(dTE.Solution.FullName);

				while (!string.IsNullOrEmpty(directory))
				{
					if (File.Exists(Path.Combine(directory, ".luarc.json")))
					{
						break;
					}

					directory = Directory.GetParent(directory)?.FullName;
				}

				return directory ?? dTE.Solution.FullName;

            }
		}
    }
}