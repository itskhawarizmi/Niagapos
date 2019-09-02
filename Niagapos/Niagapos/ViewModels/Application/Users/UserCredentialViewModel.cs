using Niagapos.Core;
using System;
using System.Windows.Threading;

namespace Niagapos
{
    public class UserCredentialViewModel : BaseViewModel
    {
        public UserCredentialViewModel()
        {
            DateTimeRealTime();
        }
        public string Name { get; set; }

        public string Caption { get; set; }

        public string Date { get; set; }

        private void DateTimeRealTime()
        {
            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            timer.Tick += (s, e) =>
            {

                Date = DateTime.Now.ToString();

            };

            timer.Start();
        }

    }
}
