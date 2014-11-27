using System.Composition;
using ExtendedInterfaces.Pcl;

namespace SimpleCalculator3
{
    [Export(typeof(IOperation))]
    [ExportMetadata("Symbol", '*')]
    class Multiply : IOperation
    {
        public int Operate(int left, int right)
        {
            return left * right;
        }

        //public char Symbol
        //{
        //    get { return '*'; }
        //}
    }

    [Export(typeof(IOperation))]
    [ExportMetadata("Symbol", '/')]
    class Divide : IOperation
    {
        public int Operate(int left, int right)
        {
            return left / right;
        }

        //public char Symbol
        //{
        //    get { return '/'; }
        //}
    }
}
