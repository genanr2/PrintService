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
using System.IO;
using System.Threading;
using System.Drawing;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Windows;
//using Microsoft.Win32
namespace ServiceGleb1
{
	public partial class ServiceGleb1 : ServiceBase
  {
    public int aaa;
    public EventLog myLog;
    public TcpListener server22;
    Int32 port;

		[DllImport("user32.dll")]
		private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string className, string windowName);

		static private Font printFont;
		static private StreamReader streamToPrint;
		static public NetworkStream stream;
		public enum SimpleServiceCustomCommands { StopWorker = 128, RestartWorker, CheckWorker, WithPrint, WithoutPrint };
		/*
				static public TcpListener server = null;
		*/
		static public Socket listenerSocket;
		public static ManualResetEvent tcpClientConnected = new ManualResetEvent(false);
		static public bool WithPrint;

    private TcpListener tcpListener;
    private Thread listenThread;
		public ServiceGleb1()
    {
      InitializeComponent();
      if (!EventLog.SourceExists("ServiceGleb")) { EventLog.CreateEventSource("Печать заявок", "ServiceGleb"); }
      myLog = new EventLog(); myLog.Source = "Печать заявок";
			WithPrint = false;
      /*
            myLog = new EventLog();myLog.Source = "Печать заявок класса ServiceGleb1";
            port = 12001;
            IPAddress localAddr = IPAddress.Any;
            this.tcpListener = new TcpListener(localAddr, port);
            this.listenThread = new Thread(new ThreadStart(ListenForClients));
            myLog.WriteEntry("ServiceGleb1: Сервер прослушивания создан. IP: " + localAddr + " Порт: " + port, EventLogEntryType.Warning, 1, 1);
            this.listenThread.Start();
      */
    }
    private void ListenForClients()
    {
      this.tcpListener.Start();
      while (true)
      {
        //blocks until a client has connected to the server
        TcpClient client = this.tcpListener.AcceptTcpClient();
        //create a thread to handle communication with connected client
        Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
        myLog.WriteEntry("ServiceGleb1: Слушаю соединение с ." +
              IPAddress.Parse(((IPEndPoint)tcpListener.LocalEndpoint).Address.ToString()) + " по порту " +
              ((IPEndPoint)tcpListener.LocalEndpoint).Port.ToString(), EventLogEntryType.Warning, 1, 1);
        clientThread.Start(client);
//				this.ShowInTaskbar = true;
      }
    }
    private void HandleClientComm(object client)
    {
      TcpClient tcpClient = (TcpClient)client;
      NetworkStream clientStream = tcpClient.GetStream();
      byte[] message = new byte[4096];
      string data;
      int bytesRead;
      while (true)
      {
        bytesRead = 0;
        //blocks until a client sends a message
        try{bytesRead = clientStream.Read(message, 0, 4096);}
        catch
        {
          //a socket error has occured
          myLog.WriteEntry("ServiceGleb1: Не могу прочитать сообщение клиента: " + tcpClient.ToString() + " Порт: " + port, EventLogEntryType.Warning, 1, 1);
          break;
        }
        if (bytesRead == 0)
        {
          //the client has disconnected from the server
          myLog.WriteEntry("ServiceGleb1: Прочитано от клиента 0 байт: " + tcpClient.ToString() + " Порт: " + port, EventLogEntryType.Warning, 1, 1);
          break;
        }
        //message has successfully been received
        ASCIIEncoding encoder = new ASCIIEncoding();
        UTF8Encoding utf8Encoder = new UTF8Encoding();
        System.Diagnostics.Debug.WriteLine(utf8Encoder.GetString(message, 0, bytesRead));
				myLog.WriteEntry("Диагностика ServiceGleb1 Прочитано: " + utf8Encoder.GetString(message, 0, bytesRead), EventLogEntryType.Warning, 1, 1);
        data = utf8Encoder.GetString(message, 0, bytesRead);
				data = data.ToUpper();
        Byte[] msg = utf8Encoder.GetBytes(data);//, 0, bytesRead);
				clientStream.Write(msg, 0, msg.Length);
        try
        {
          FileStream fs = null;
          try
          {
            fs = new FileStream("c:\\CDrive.txt", FileMode.Create, FileAccess.Write, FileShare.None);
          }
          catch (Exception ex)
          {
            myLog.WriteEntry("Не могу создать файл c:\\CDrive.txt " + ex.Message, EventLogEntryType.Warning, 1, 1);
          }
          StreamWriter sw = new StreamWriter(fs);
          try { sw.Write("Заявка на пропуск\n"); sw.Write(data); }finally { sw.Flush(); sw.Close(); }
        }
        catch (Exception ex)
        {
          myLog.WriteEntry("Не могу записать на диск. " + ex.Message, EventLogEntryType.Warning, 1, 1);
        }
				if(WithPrint==true)printMessage(clientStream);

      }
      tcpClient.Close();
    }
    protected override void OnStart(string[] args)
    {
//      EventLog myLog = new EventLog();
//      myLog.Source = "Печать заявок";
      myLog.WriteEntry("Сервер печати заявок заускается (OnStart).", EventLogEntryType.Warning, 1, 1);
//      myLog = new EventLog(); myLog.Source = "Печать заявок класса ServiceGleb1";
      port = 12001;
      IPAddress localAddr = IPAddress.Any;
      this.tcpListener = new TcpListener(localAddr, port);
      this.listenThread = new Thread(new ThreadStart(ListenForClients));
      myLog.WriteEntry("ServiceGleb1: Сервер прослушивания создан. IP: " + localAddr + " Порт: " + port, EventLogEntryType.Warning, 1, 1);
      this.listenThread.Start();

    }
    protected override void OnStop()
    {
//      EventLog myLog = new EventLog();
//      myLog.Source = "Печать заявок";
      myLog.WriteEntry("Сервер печати заявок остановлен (OnStop).", EventLogEntryType.Warning, 1, 1);
//      Program
    }
		protected override void OnContinue()
		{
			//      EventLog myLog = new EventLog();
			//      myLog.Source = "Печать заявок";
			myLog.WriteEntry("Продолжение работы сервера приёма заявок (OnContinue).", EventLogEntryType.Warning, 1, 1);
			//      Program
		}
		public void printMessage(NetworkStream str1)
		{
			try
			{
				streamToPrint = new StreamReader("C:\\CDrive.txt");
				try
				{
					printFont = new Font("Arial", 10);
					PrintDocument pd = new PrintDocument();
					pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
					pd.PrinterSettings.PrinterName = "Xerox WC M123 PCL";
					myLog.WriteEntry("PrinterName: " + pd.PrinterSettings.PrinterName, EventLogEntryType.Warning, 1, 1);
					try
					{
						pd.Print();
					}
					catch (Exception ex)
					{
						myLog.WriteEntry("SeerviceGleb1 Ошибка начала печати: " + ex.Message, EventLogEntryType.Error, 1, 1);
					}
				}
				finally { streamToPrint.Close(); }
			}
			catch (Exception ex)
			{
				myLog.WriteEntry("SeerviceGleb1 Ошибка печати: " + ex.Message, EventLogEntryType.Error, 1, 1);
			}
		}
		public void pd_PrintPage(object sender, PrintPageEventArgs ev)
		{
			float linesPerPage = 0;
			float yPos = 0;
			int count = 0;
			float leftMargin = ev.MarginBounds.Left;
			float topMargin = ev.MarginBounds.Top;
			string line = null;
			// Calculate the number of lines per page.
			myLog.WriteEntry("Печать на принтер5.", EventLogEntryType.Warning, 1, 1);
			linesPerPage = ev.MarginBounds.Height / printFont.GetHeight(ev.Graphics);
			// Print each line of the file.
			while (count < linesPerPage && ((line = streamToPrint.ReadLine()) != null))
			{
				yPos = topMargin + (count * printFont.GetHeight(ev.Graphics));
				ev.Graphics.DrawString(line, printFont, Brushes.Black, leftMargin, yPos, new StringFormat());
				count++;
			}
			// If more lines exist, print another page.
			if (line != null) ev.HasMorePages = true; else ev.HasMorePages = false;
		}
		protected override void OnCustomCommand(int command)
		{
#if LOGEVENTS
            EventLog.WriteEntry("SimpleService.OnCustomCommand", DateTime.Now.ToLongTimeString() + " - Custom command received: " + command.ToString());
#endif
			// If the custom command is recognized, signal the worker thread appropriately.
			switch (command)
			{
				case (int)SimpleServiceCustomCommands.WithPrint:
					WithPrint = true;
					myLog.WriteEntry("Принять заявку с последующим выводом на печать.", EventLogEntryType.Warning, 1, 1);
					break;
				case (int)SimpleServiceCustomCommands.WithoutPrint:
					WithPrint = false;
					myLog.WriteEntry("Принять заявку без последующей выдачи на печать.", EventLogEntryType.Warning, 1, 1);
					// Signal the worker thread to terminate. For this custom command, the main service continues to run without a worker thread.
//					pause.Reset();
					break;
				case (int)SimpleServiceCustomCommands.RestartWorker:
					// Restart the worker thread if necessary.
//					pause.Set();
					break;
				case (int)SimpleServiceCustomCommands.CheckWorker:
#if LOGEVENTS
				// Log the current worker thread state.
        EventLog.WriteEntry("SimpleService.OnCustomCommand", DateTime.Now.ToLongTimeString() + " OnCustomCommand - Worker thread state = " +workerThread.ThreadState.ToString());
#endif
					break;
				default:
#if LOGEVENTS
                    EventLog.WriteEntry("SimpleService.OnCustomCommand",DateTime.Now.ToLongTimeString());
#endif
					break;
			}
		}

	}
}
