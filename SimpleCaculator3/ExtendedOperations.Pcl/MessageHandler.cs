using System.Composition;
using ExtendedInterfaces.Pcl;

namespace ExtendedOperations.Pcl
{
    [Export(typeof(IMessageHandler))]
    public class MessageHandler : IMessageHandler
    {
        [ImportingConstructor]
        public MessageHandler(IDatabaseConnection connection)
        {
        }
    }
}
