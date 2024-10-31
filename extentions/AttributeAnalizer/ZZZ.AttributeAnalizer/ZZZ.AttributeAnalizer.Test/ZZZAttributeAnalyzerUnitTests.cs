using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Threading.Tasks;
using VerifyCS = ZZZ.AttributeAnalyzer.Test.CSharpCodeFixVerifier<
    ZZZ.AttributeAnalyzer.ZZZAttributeAnalyzerAnalyzer,
    ZZZ.AttributeAnalyzer.ZZZAttributeAnalyzerCodeFixProvider>;

namespace ZZZ.AttributeAnalyzer.Test
{
    [TestClass]
    public class ZZZAttributeAnalizerUnitTest
    {
        //No diagnostics expected to show up
        [TestMethod]
        public async Task TestMethod1()
        {
            var test = @"";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        //Diagnostic and CodeFix both triggered and checked for
        [TestMethod]
        public async Task TestMethod2()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class {|#0:TypeName|}
        {   
        }
    }";

            var fixtest = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TYPENAME
        {   
        }
    }";

            if (!Debugger.IsAttached)
                Debugger.Launch();

            var expected = VerifyCS.Diagnostic("ZZZAttributeAnalizer").WithLocation(0).WithArguments("TypeName");
            await VerifyCS.VerifyCodeFixAsync(test, expected, fixtest);
        }
    }
}
