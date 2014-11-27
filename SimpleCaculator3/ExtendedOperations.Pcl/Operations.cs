using System.Composition;
using ExtendedInterfaces.Pcl;

namespace ExtendedOperations
{
    [Export(typeof(IOperation))]
    class Mod : IOperation
    {
        public int Operate(int left, int right)
        {
            return left % right;
        }

        public char Symbol
        {
            get { return '%'; }
        }
    }

    [Export(typeof(IOperation))]
    class Add : IOperation
    {
        public int Operate(int left, int right)
        {
            return left + right;
        }

        #region IOperation Members

        public char Symbol
        {
            get { return '+'; }
        }

        #endregion
    }

    [Export(typeof(IOperation))]
    class Subtract : IOperation
    {
        public int Operate(int left, int right)
        {
            return left - right;
        }

        #region IOperation Members

        public char Symbol
        {
            get { return '-'; }
        }

        #endregion
    }

}
