using System.Composition;
using ExtendedInterfaces.Pcl;

namespace ExtendedOperations.Pcl
{
    [Export(typeof(IDatabaseConnection))]
    class DatabaseConnection : IDatabaseConnection
    {
    }
}
