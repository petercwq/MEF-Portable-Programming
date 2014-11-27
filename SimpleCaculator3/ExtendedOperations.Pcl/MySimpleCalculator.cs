using System;
using System.Collections.Generic;
using System.Composition;
using ExtendedInterfaces.Pcl;

namespace ExtendedOperations.Pcl
{
    [Export(typeof(ICalculator))]
    class MySimpleCalculator : ICalculator, IDisposable
    {
        [ImportMany]
        public IEnumerable<ExportFactory<IOperation, OperationMetadata>> OperationsTest { get; private set; }

        //private readonly IEnumerable<IOperation> operations;

        private readonly IEnumerable<KeyValuePair<IOperation, OperationMetadata>> operations;

        private readonly IMessageHandler handler;

        [ImportingConstructor]
        public MySimpleCalculator([Import(AllowDefault = true)] IMessageHandler handler, [ImportMany] IEnumerable<ExportFactory<IOperation, OperationMetadata>> operations)
        {
            this.handler = handler;
            var list = new List<KeyValuePair<IOperation, OperationMetadata>>();
            foreach (var op in operations)
            {
                var ef = op.CreateExport();
                list.Add(new KeyValuePair<IOperation, OperationMetadata>(ef.Value, op.Metadata));
                ef.Dispose();
            }
            this.operations = list;
        }

        public String Calculate(String input)
        {
            int left;
            int right;
            Char operation;
            int fn = FindFirstNonDigit(input); //finds the operator
            if (fn < 0) return "Could not parse command.";

            try
            {
                //separate out the operands
                left = int.Parse(input.Substring(0, fn));
                right = int.Parse(input.Substring(fn + 1));
            }
            catch
            {
                return "Could not parse command.";
            }

            operation = input[fn];

            foreach (var i in operations)
            {
                if (i.Value.Symbol.Equals(operation)) return i.Key.Operate(left, right).ToString();
            }
            return "Operation Not Found!";
        }

        private int FindFirstNonDigit(String s)
        {

            for (int i = 0; i < s.Length; i++)
            {
                if (!(Char.IsDigit(s[i]))) return i;
            }
            return -1;
        }

        #region IDisposable Members

        public void Dispose()
        {
            foreach(var item in operations)
            {
                
            }
        }

        #endregion
    }
}
