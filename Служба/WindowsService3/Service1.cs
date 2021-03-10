using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Net; 
using System.Net.Sockets;
namespace WindowsService3
{
  public partial class Service1 : ServiceBase
  {
    public int aaa;
    public EventLog myLog;
    public TcpListener server22;
    public Service1()
    {
      InitializeComponent();
      myLog = new EventLog();
      myLog.Source = "Печать заявок";
    }
    protected override void OnStart(string[] args)
    {
//      EventLog myLog = new EventLog();
//      myLog.Source = "Печать заявок";
      myLog.WriteEntry("Сервер печати заявок заускается (OnStart).", EventLogEntryType.Warning, 1, 1);
    }
    protected override void OnStop()
    {
//      EventLog myLog = new EventLog();
//      myLog.Source = "Печать заявок";
      myLog.WriteEntry("Сервер печати заявок остановлен (OnStop).", EventLogEntryType.Warning, 1, 1);
//      Program
    }
  }
}
