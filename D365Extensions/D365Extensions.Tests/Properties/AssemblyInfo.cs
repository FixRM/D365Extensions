using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ExecutionScope = Microsoft.VisualStudio.TestTools.UnitTesting.ExecutionScope;

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("57ad9ed0-3237-44b4-bab6-d80870cc46d9")]

// For running each method in separate thread
// https://devblogs.microsoft.com/devops/mstest-v2-in-assembly-parallel-test-execution/
[assembly: Parallelize(Workers = 0, Scope = ExecutionScope.MethodLevel)]