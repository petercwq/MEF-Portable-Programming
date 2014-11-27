using System;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Hosting;
using System.Reflection;
using ExtendedInterfaces.Pcl;

namespace SimpleCalculator3
{
    [Export]
    class Configuration
    {
        [Export("HandlerName")]
        public string HandlerName { get { return "New Handler"; } }
    }

    class Program
    {
        [Import]
        public ICalculator calculator { get; set; }

        private Program()
        {
            var configuration = new ContainerConfiguration()
                .WithAssembly(typeof(Program).Assembly);

            //var conventions = new ConventionBuilder();
            //conventions.ForTypesDerivedFrom<IMessageHandler>().Export();
            //configuration.WithPart<MessageHandler>(conventions);

            using (var container = configuration.CreateContainer())
            {
                //Fill the imports of this object
                try
                {
                    container.SatisfyImports(this);
                }
                catch (CompositionFailedException compositionException)
                {
                    // no export error
                    System.Diagnostics.Debug.WriteLine(compositionException.ToString());
                }
            }

            configuration = configuration.WithAssemblies(GetAssembliesFromDirectory(@".\Extensions", "*.Pcl.dll"));

            using (var container = configuration.CreateContainer())
            {
                try
                {
                    //test satisfy again
                    container.SatisfyImports(this);
                }
                catch (CompositionFailedException compositionException)
                {
                    Console.WriteLine(compositionException.ToString());
                }
            }
        }

        static void Main(string[] args)
        {
            Program p = new Program(); //Composition is performed in the constructor
            String s;
            Console.WriteLine("Enter Command:");
            while (true)
            {
                s = Console.ReadLine();
                Console.WriteLine(p.calculator.Calculate(s));
            }
        }


        /// <summary>
        /// Gets a list of assemblies from a directory
        /// </summary>
        /// <param name="Directory">The directory to search in</param>
        /// <returns>List of assemblies in the directory</returns>
        public static IEnumerable<Assembly> GetAssembliesFromDirectory(string Directory, string filter)
        {
            var Files = System.IO.Directory.GetFiles(Directory, filter, System.IO.SearchOption.AllDirectories);
            foreach (var File in Files)
            {
                yield return Assembly.LoadFile(System.IO.Path.GetFullPath(File));
            }
        }
    }
}
