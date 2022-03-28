using Newtonsoft.Json;
using System;
using System.Linq;
using System.ServiceProcess;
using System.Timers;
using WindowsService.Model;

namespace WindowsService
{
    public partial class Service1 : ServiceBase
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(Service1));
        private Timer timer = null;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // Ghi vào log file khi services dc start lần đầu tiên
            log.Debug("Service On Start!");
            // Tạo 1 timer từ libary System.Timers
            timer = new Timer
            {
                // Execute mỗi 60s
                Interval = 30000
            };
            // Những gì xảy ra khi timer đó dc tick
            timer.Elapsed += Timer_Tick;
            // Enable timer
            timer.Enabled = true;
        }

        private void Timer_Tick(object sender, ElapsedEventArgs args)
        {
            try
            {
                Doing();
            }
            catch (Exception exx)
            {
                log.Debug("exx: " + exx.StackTrace);
            }
        }

        private void Doing()
        {
            log.Debug("Doing...");
        }

        protected override void OnStop()
        {
            // Ghi log lại khi Services đã được stop
            timer.Enabled = true;
            log.Debug("Service On Stop!");
        }
    }
}
