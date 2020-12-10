using System.Reflection;
using System.Runtime.InteropServices;

// It is common, and mostly good, to use one GlobalAssemblyInfo.cs that is added 
// as a link to many projects of the same product, details below
// Change these attribute values in local assembly info to modify the information.
[assembly: AssemblyProduct("Gofer")]
[assembly: AssemblyCompany("Gowon, Ltd.")]
[assembly: AssemblyCopyright("Copyright © Gowon, Ltd. 2020")]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("6f2f524a-d4b4-4426-8f9f-8d7089748a02")]
[assembly: ComVisible(false)] //not going to expose ;)

// Version information for an assembly consists of the following four values:
// roughly translated from I reckon it is for SO, note that they most likely 
// dynamically generate this file
//      Major Version  - Year 6 being 2016
//      Minor Version  - The month
//      Day Number     - Day of month
//      Revision       - Build number
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below: [assembly: AssemblyVersion("year.month.day.*")]
[assembly: AssemblyVersion(ThisAssembly.Version)]
[assembly: AssemblyFileVersion(ThisAssembly.SimpleVersion)]
[assembly: AssemblyInformationalVersion(ThisAssembly.InformationalVersion)]

// ReSharper disable once CheckNamespace
internal partial class ThisAssembly
{
    /// <summary>
    ///     Simple release-like version number, like 4.0.1 for a cycle 5, SR1 build.
    /// </summary>
    public const string SimpleVersion =
        Git.BaseVersion.Major + "." + Git.BaseVersion.Minor + "." + Git.BaseVersion.Patch;

    /// <summary>
    ///     Full version, including commits since base version file, like 4.0.1.598
    /// </summary>
    public const string Version = SimpleVersion + "." + Git.Commits;

    /// <summary>
    ///     Full version, plus branch and commit short sha, like 4.0.1.598-cycle6+39cf84e
    /// </summary>
    public const string InformationalVersion = Version + "-" + Git.Branch + "+" + Git.Commit;
}