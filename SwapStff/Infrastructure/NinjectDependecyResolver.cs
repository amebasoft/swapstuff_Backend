using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using Ninject.Web.Common;

namespace SwapStff.Infrastructure
{
    public interface IDependencyResolver
    {
        object GetService(Type serviceType);
        IEnumerable<object> GetServices(Type serviceType);
    }
    public class NinjectDependecyResolver : IDependencyResolver
    {
        IKernel kernal;

        public NinjectDependecyResolver()
        {
            kernal = new StandardKernel();
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernal.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernal.GetAll(serviceType);
        }

        void AddBindings()
        {
            //kernal.Bind<ICalculator>().To<EngineeringCalculator>();
    
        }
    }
}