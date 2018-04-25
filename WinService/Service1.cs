using Common;
using System;
using System.ServiceProcess;

namespace WinService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Elapsed += Timer_Elapsed;
            timer.Interval = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings.Get("Interval"));
            timer.Enabled = true;
        }
        public void Timer_Elapsed(object sender,System.Timers.ElapsedEventArgs e)
        {
            var datetime = DateTime.Now;
            if(datetime.Minute %5==0 && datetime.Second==1)
            {
                Log.i("Hello World");
            }
        }
        protected override void OnStart(string[] args)
        {
            Log.i("WinService启动");
        }
        protected override void OnStop()
        {
            Log.i("WinService停止");
        }
        protected override void OnShutdown()
        {
            Log.i("WinService停止：计算机关闭");
        }
    }
}
