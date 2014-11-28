using System;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Hosting;
using System.Reflection;
using ExtendedInterfaces.Pcl;

namespace SimpleCalculator3
{
    /*
     * Because parts are hosted in the composition container, their life cycle can be more complex than ordinary objects. Parts can implement two important life cycle-related interfaces: IDisposable and IPartImportsSatisfiedNotification. 
     * 
     * Parts that require work to be performed at shut down or that need to release resources should implement IDisposable, as usual for .NET Framework objects. However, since the container creates and maintains references to parts, only the container that owns a part should call the Dispose method on it. The container itself implements IDisposable, and as portion of its cleanup in Dispose it will call Dispose on all the parts that it owns. For this reason, you should always dispose the composition container when it and any parts it owns are no longer needed.
     * 
     * For long-lived composition containers, memory consumption by parts with a creation policy of non-shared can become a problem. These non-shared parts can be created multiple times and will not be disposed until the container itself is disposed. To deal with this, the container provides the ReleaseExport method. Calling this method on a non-shared export removes that export from the composition container and disposes it. Parts that are used only by the removed export, and so on down the tree, are also removed and disposed. In this way, resources can be reclaimed without disposing the composition container itself.
     * 
     * IPartImportsSatisfiedNotification contains one method named OnImportsSatisfied. This method is called by the composition container on any parts that implement the interface when composition has been completed and the part's imports are ready for use. Parts are created by the composition engine to fill the imports of other parts. Before the imports of a part have been set, you cannot perform any initialization that relies on or manipulates imported values in the part constructor unless those values have been specified as prerequisites by using the ImportingConstructor attribute. This is normally the preferred method, but in some cases, constructor injection may not be available. In those cases, initialization can be performed in OnImportsSatisfied, and the part should implement IPartImportsSatisfiedNotification. 
     */

    [Export]
    class Configuration
    {
        [Export("HandlerName")]
        public string HandlerName { get { return "New Handler"; } }
    }

    [Flags]
    public enum Platforms
    {
        Windows,
        Android
    }

    //design for export
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public class BslExportAttribute : ExportAttribute
    {
        public BslExportAttribute(Type contractType, Platforms platforms)
            : base(contractType)
        {
            SupportedPlatforms = platforms;
        }

        public BslExportAttribute(string contractName, Platforms platforms)
            : base(contractName)
        {
            SupportedPlatforms = platforms;
        }

        public Platforms SupportedPlatforms { get; private set; }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    public class BslImportAttribute : ImportAttribute
    {
        public BslImportAttribute(string contractName)
            : base(contractName)
        {
        }

        public BslImportAttribute()
        { }
    }

    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public class LslExportAttribute : ExportAttribute
    {
        public LslExportAttribute(Type contractType)
            : base(contractType)
        {

        }

        public LslExportAttribute(string contractName)
            : base(contractName)
        {

        }
    }

    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public class ServiceExportAttribute : ExportAttribute
    {
        public ServiceExportAttribute(Type contractType)
            : base(contractType)
        {

        }

        public ServiceExportAttribute(string contractName)
            : base(contractName)
        {

        }
    }

    // design for share
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
    public class Lsl1Attribute : SharedAttribute
    {
        public Lsl1Attribute()
            : base() //for globally shared
        {

        }
    }

    public interface IBluetooth
    {

    }

    [BslExport(typeof(IBluetooth), Platforms.Windows | Platforms.Android)]
    public class Bluetooth : IBluetooth
    {

    }

    //public class Provider: System.Composition.Hosting.Core.ExportDescriptorProvider
    //{

    //}

    class Program
    {
        [Import]
        public ICalculator calculator { get; set; }

        [Import]
        public ExportFactory<ICalculator> exportCalculator { get; set; }

        [BslImport]
        public IBluetooth bluetooth { get; set; }

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

                    //test exprot factory
                    using (var export = exportCalculator.CreateExport())
                    {
                        var calc1 = export.Value;
                    }

                    // test container export
                    var calc2 = container.GetExport<ICalculator>();
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
