using System.Composition;
using ExtendedInterfaces.Pcl;

namespace ExtendedOperations
{
    [Export(typeof(IOperation))]
    [ExportMetadata("Symbol", '%')]
    class Mod : IOperation
    {
        public int Operate(int left, int right)
        {
            return left % right;
        }

        //public char Symbol
        //{
        //    get { return '%'; }
        //}
    }

    [Export(typeof(IOperation))]
    [ExportMetadata("Symbol", '+')]
    class Add : IOperation
    {
        public int Operate(int left, int right)
        {
            return left + right;
        }

        //public char Symbol
        //{
        //    get { return '+'; }
        //}
    }

    [Export(typeof(IOperation))]
    [ExportMetadata("Symbol", '-')]
    class Subtract : IOperation
    {
        public int Operate(int left, int right)
        {
            return left - right;
        }

        //public char Symbol
        //{
        //    get { return '-'; }
        //}
    }

}
