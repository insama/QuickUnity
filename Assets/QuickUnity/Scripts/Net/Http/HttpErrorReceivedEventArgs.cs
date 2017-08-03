using System;

namespace QuickUnity.Net.Http
{
    public class HttpErrorReceivedEventArgs : EventArgs
    {
        private string errorMessage;

        public HttpErrorReceivedEventArgs(string errorMessage)
            : base()
        {
            this.errorMessage = errorMessage;
        }

        public string ErrorMessage
        {
            get { return errorMessage; }
        }
    }
}