using System;
using System.Dynamic;

namespace SimpleBDD
{

    public class GivenWhenThenFixture : IDisposable
    {
        public dynamic Given { get; set; }
        public dynamic When { get; set; }
        public object Then { get { return When; }  }

        public GivenWhenThenFixture()
        {
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