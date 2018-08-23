using System;
using System.Dynamic;
using static System.Console;

namespace CoreBDD
{
    [Obsolete("Please use SpecFixture", true)]
    public class GivenWhenThenFixture : SpecFixture
    {

    }

    public class SpecFixture : IDisposable
    {
        public dynamic Given { get; set; }
        public dynamic When { get; set; }
        public Feature Feature { get; internal set; }

        public object Then { get { return When; }  }
        public object Result { get { return When; }  }

        public SpecFixture()
        {
            if (Feature != null)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(Feature.Description);
                ResetColor();

            }
            Given = new ExpandoObject();
            When = new ExpandoObject();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}