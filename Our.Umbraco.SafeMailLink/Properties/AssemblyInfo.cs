using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Web;
using Our.Umbraco.SafeMailLink.Events;

// General Information about an assembly is controlled through the following set of attributes.
// Change these attribute values to modify the information associated with an assembly.
[assembly: AssemblyTitle("Our.Umbraco.SafeMailLink")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Umbrella Inc Ltd")]
[assembly: AssemblyProduct("Our.Umbraco.SafeMailLink")]
[assembly: AssemblyCopyright("Copyright (c) Umbrella Inc Ltd 2011")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible to COM components.
// If you need to access a type in this assembly from COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("46850d7f-f14e-469a-8d8b-b476f2995f0e")]

// Version information for an assembly consists of the following four values:
// [Major].[Minor].[Build].[Revision]
[assembly: AssemblyVersion("1.0.1.0")]
[assembly: AssemblyFileVersion("1.0.1.0")]

[assembly: PreApplicationStartMethod(typeof(ApplicationEventsHandler), "RegisterModules")]