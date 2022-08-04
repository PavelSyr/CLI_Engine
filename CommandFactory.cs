using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CLI_Engine
{
    public class CommandFactory : ICommandFactory
    {
        private readonly IParamsFactory _paramsFactory;
        private readonly Assembly _assembly;
        private readonly string _nameTemplate;

        public CommandFactory(Assembly assembly, string nameTemplate, FactoryMethod factoryMethod) : this(assembly, nameTemplate, new DelegateParamsFactory(factoryMethod))
        {
        }
        
        public CommandFactory(Assembly assembly, string nameTemplate, IParamsFactory paramsFactory)
        {
            _assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));

            _nameTemplate = nameTemplate ?? throw new ArgumentNullException(nameof(nameTemplate));
            
            _paramsFactory = paramsFactory ?? throw new ArgumentNullException(nameof(paramsFactory));
        }

        public ICommand Create(string[] args)
        {
            var commands = GetTypesAttribute();

            if (args.Length <= 0)
            {
                return null;
            }
            
            var cmdStr = args[0];
            var (type, _) = commands
                .FirstOrDefault(c => c.attrib.Name.Equals(cmdStr));

            if (string.IsNullOrEmpty(type))
            {
                return null;
            }

            var paramsArray = _paramsFactory.Create(args);
            
            var cmdObj = _assembly.CreateInstance(
                type,
                false,
                BindingFlags.Public | BindingFlags.Instance,
                binder: null,
                args: paramsArray,
                culture: null,
                activationAttributes: null);

            return cmdObj as ICommand;
        }
        
        private IEnumerable<(string type, CommandAttribute attrib)> GetTypesAttribute()
        {
            return from type in _assembly.GetTypes().Where(t => t.Name.StartsWith(_nameTemplate))
                let attribs = type.GetCustomAttributes(typeof(CommandAttribute), true)
                where attribs.Length > 0
                select (type.FullName, attribs.First() as CommandAttribute);
        }
    }
}