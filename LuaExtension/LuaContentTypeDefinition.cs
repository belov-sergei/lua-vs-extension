using Microsoft.VisualStudio.Utilities;
using Microsoft.VisualStudio.LanguageServer.Client;
using System.ComponentModel.Composition;

namespace LuaExtension
{
	public class LuaContentTypeDefinition
	{
		[Export] [Name("Lua")] [BaseDefinition(CodeRemoteContentDefinition.CodeRemoteContentTypeName)]
		internal static ContentTypeDefinition ContentTypeDefinition;

		[Export] [FileExtension(".lua")] [ContentType("Lua")]
		internal static FileExtensionToContentTypeDefinition FileExtensionDefinition;
	}
}