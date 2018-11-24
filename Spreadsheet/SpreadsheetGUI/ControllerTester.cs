using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSGui
{
    [TestClass]
    class ControllerTester
    {
        [TestMethod]
        public void SSGuiTestMethod1()
        {
            SpreadsheetStub stub = new SpreadsheetStub();
            Controller controller = new Controller(stub);
            stub.FireCloseEvent();
            Assert.IsTrue(stub.CalledDoClose);
            stub.FireOpenEvent();
            Assert.IsTrue(stub.CalledDoOpen);
            stub.FireOpenEvent();
            Assert.IsTrue(stub.CalledOpenNew);
        }

        [TestMethod]
        public void SSGuiTestMethod2()
        {
            SpreadsheetStub stub = new SpreadsheetStub();
            Controller controller = new Controller(stub,"TestSS.ss");
            Assert.IsTrue(stub.CellName == "A1");
            Assert.IsTrue(stub.CellValue == "88");
        }
    }
}
