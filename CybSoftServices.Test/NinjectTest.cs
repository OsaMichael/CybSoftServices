using CybSoftServices.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CybSoftServices.Test
{
    [TestClass]
    public class NinjectTest
    {
        [TestMethod]
        public void TestNinjectBindings()
        {
            //Create Kernel
            var kernel = new StandardKernel();

            var assembly = Assembly.Load("CybSoftServices");

            kernel.Load(assembly);
            kernel.Get<IServiceManager>();
            //kernel.Get<IVoterManager>();

            // best way to do this
            var interfaces = assembly.GetTypes()
                                    .Where(t => t.FullName.StartsWith("CybSoftServices.Interface"))
                                    .Where(t => t.IsInterface).ToList();

            foreach (var iInterface in interfaces)
            {
                kernel.Get(iInterface);
            }
        }
    }
}
