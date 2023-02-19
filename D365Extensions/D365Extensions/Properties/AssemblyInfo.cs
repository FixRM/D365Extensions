using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("0fd54081-45d9-41c1-8732-13034bdbb8e6")]

#if DEBUG
[assembly: InternalsVisibleTo("D365Extensions.Tests")]
#else
[assembly: InternalsVisibleTo("D365Extensions.Tests, PublicKey=00240000048000009400000006020000002400005253413100040000010001004d75870d5da62e09344a1dcd5a3e5006ff4c8a6b7c13560988e21b11a29b11bcf0f650eabccb11523898199ce9403c5132e1ae77150a964c0d7462b7aaf3e4acd6cb0c1f85ce4fa5152377c9bd2e2cd9992474c99669614c07d6b85440444885ed7a526f6398bf8d3eb4ad58d6cebe996dccc6e8c2ab07394f51c3fdc3dcacc1")]
#endif
