using System.Composition;
using ExtendedInterfaces.Pcl;

namespace ExtendedOperations.Pcl
{
    [Export(typeof(IMessageHandler))]
    public class MessageHandler : IMessageHandler
    {
        private readonly IDatabaseConnection connection;
        private readonly string name;

        [ImportingConstructor]
        public MessageHandler(IDatabaseConnection connection, [Import("HandlerName")] string name)
        {
            this.connection = connection;
            this.name = name;
        }
    }
}
