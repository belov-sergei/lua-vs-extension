using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using Task = System.Threading.Tasks.Task;

namespace LuaExtension
{
	[PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
	[Guid(LuaExtensionPackage.PackageGuidString)]
	public sealed class LuaExtensionPackage : AsyncPackage
	{
		public const string PackageGuidString = "e664e89e-adff-4e52-9ade-4642795b2c30";

		protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
		{
			await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
		}
	}
}