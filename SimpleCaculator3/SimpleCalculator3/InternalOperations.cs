using System.Composition;
using ExtendedInterfaces.Pcl;

namespace SimpleCalculator3
{
    [Export(typeof(IOperation))]
    class Multiply : IOperation
    {
        public int Operate(int left, int right)
        {
            return left * right;
        }

        public char Symbol
        {
            get { return '*'; }
        }
    }

    [Export(typeof(IOperation))]
    class Divide : IOperation
    {
        public int Operate(int left, int right)
        {
            return left / right;
        }

        public char Symbol
        {
            get { return '/'; }
        }
    }
}
