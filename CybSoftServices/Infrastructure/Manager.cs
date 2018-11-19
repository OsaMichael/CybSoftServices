using CybSoftServices.Interface;
using CybSoftServices.Manager;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CybSoftServices.Infrastructure
{
    public class Manager : NinjectModule
    {
        public override void Load()
        {

            Bind<IServiceManager>().To<ServiceManager>();
           // Bind<IProjectManager>().To<ProjectManager>();

        }
    }
}