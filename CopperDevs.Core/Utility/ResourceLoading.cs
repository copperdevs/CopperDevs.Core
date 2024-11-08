using System.Reflection;
using System.Text;

namespace CopperDevs.Core.Utility;

/// <summary>
/// Utility class for loading manifest resources from an assembly
/// </summary>
public static class ResourceLoading
{
    /// <summary>
    /// Load a resource from an assembly as a byte array
    /// </summary>
    /// <param name="targetAssembly">Assembly to load the resource from</param>
    /// <param name="fullPath">Full path of the resource</param>
    /// <returns>Target resource as a byte array</returns>
    public static byte[] LoadAsset(Assembly targetAssembly, string fullPath)
    {
        var stream = targetAssembly.GetManifestResourceStream(fullPath);

        using var ms = new MemoryStream();

        stream?.CopyTo(ms);

        return ms.ToArray();
    }

    /// <summary>
    /// Load a resource from an assembly as a string
    /// </summary>
    /// <param name="targetAssembly">Assembly to load the resource from</param>
    /// <param name="fullPath">Full path of the resource</param>
    /// <returns>Target resource as a string</returns>
    public static string LoadTextAsset(Assembly targetAssembly, string fullPath)
    {
        var bytes = LoadAsset(targetAssembly, fullPath);

        return Encoding.Default.GetString(bytes, 0, bytes.Length);
    }
}

/// <summary>
/// Extensions for the <see cref="ResourceLoading"/> so you can load assets as extensions on <see cref="Assembly"/>
/// </summary>
public static class ResourceLoadingExtensions
{
    /// <summary>
    /// Load a resource from an assembly as a byte array
    /// </summary>
    /// <param name="targetAssembly">Assembly to load the resource from</param>
    /// <param name="fullPath">Full path of the resource</param>
    /// <returns>Target resource as a byte array</returns>
    public static byte[] LoadAsset(this Assembly targetAssembly, string fullPath) => ResourceLoading.LoadAsset(targetAssembly, fullPath);

    /// <summary>
    /// Load a resource from an assembly as a string
    /// </summary>
    /// <param name="targetAssembly">Assembly to load the resource from</param>
    /// <param name="fullPath">Full path of the resource</param>
    /// <returns>Target resource as a string</returns>

    public static string LoadTextAsset(this Assembly targetAssembly, string fullPath) => LoadTextAsset(targetAssembly, fullPath);
}
