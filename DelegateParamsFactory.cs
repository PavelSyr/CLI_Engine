using System;

namespace CLI_Engine
{
    public class DelegateParamsFactory : IParamsFactory
    {
        private readonly FactoryMethod _factory;
        
        public DelegateParamsFactory(FactoryMethod factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }
        
        public object[] Create(string[] args)
        {
            return _factory.Invoke(args);
        }
    }
}