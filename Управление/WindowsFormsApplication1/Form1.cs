using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Diagnostics;
using System.Threading;
using System.Drawing.Printing;
using System.Diagnostics;
using System.Threading;
using System.ServiceProcess;
//using System.Configuration;



namespace WindowsFormsApplication1
{
    public partial class PrintServerLink : Form
    {
        //    string serverName;
        //    Font printFont;
        public Font printFont;
        public StreamReader streamToPrint;
        public NetworkStream stream = null;
        public ServiceController[] scServices;
        public enum SimpleServiceCustomCommands { StopWorker = 128, RestartWorker, CheckWorker };

        public PrintServerLink() { InitializeComponent(); }
        private void SendButton_Click(object sender, EventArgs e)
        {
            //        Connect("server2", "Всем привет!!!");
            Connect("192.168.1.122", "Всем привет!!!");
        }
        void printMessage(NetworkStream str1)
        {
            EventLog myLog = new EventLog();
            try
            {
                myLog.Source = "Печать заявок";
                myLog.WriteEntry("Печать на принтер 1.", EventLogEntryType.Warning, 1, 1);
                streamToPrint = new StreamReader("C:\\CDrive.txt");
                myLog.WriteEntry("Печать на принтер 2.", EventLogEntryType.Warning, 1, 1);
                //        streamToPrint = str1;
                try
                {
                    printFont = new Font("Arial", 10);
                    PrintDocument pd = new PrintDocument();
                    //pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
                    PrintPageEventHandler eh = new PrintPageEventHandler(pd_PrintPage);
                    //pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
                    pd.PrintPage += eh;


                    myLog.WriteEntry("PrintPageEventHandler: " + eh.ToString(), EventLogEntryType.Warning, 1, 1);
                    myLog.WriteEntry("Печать на принтер 3: " + pd.DocumentName, EventLogEntryType.Warning, 1, 1);
                    //            pd.Print();
                    pd.PrintController = new PrintControllerWithStatusDialog(pd.PrintController);
                    myLog.WriteEntry("PrinterName: " + pd.PrinterSettings.PrinterName, EventLogEntryType.Warning, 1, 1);
                    myLog.WriteEntry("PrintFileName: " + pd.PrinterSettings.PrintFileName, EventLogEntryType.Warning, 1, 1);
                    myLog.WriteEntry("PrintToFile? " + pd.PrinterSettings.PrintToFile, EventLogEntryType.Warning, 1, 1);
                    myLog.WriteEntry("PrinterSettings: " + pd.PrinterSettings.ToString(), EventLogEntryType.Warning, 1, 1);
                    try
                    {
                        //							pd.Print();
                        PrintEventArgs ev = new PrintEventArgs();
                        //ev.PrintAction = PrintAction.PrintToFile;
                        pd.PrintController.OnStartPrint(pd, ev);
                        //pd.Print();
                        //pd.PrintController
                    }
                    catch (Exception ex)
                    {
                        myLog.WriteEntry("Ошибка начала печати: " + ex.Message, EventLogEntryType.Error, 1, 1);
                    }
                    myLog.WriteEntry("Печать на принтер 4.", EventLogEntryType.Warning, 1, 1);
                }
                finally { streamToPrint.Close(); }
            }
            catch (Exception ex)
            {/*MessageBox.Show(ex.Message);*/
                myLog.WriteEntry("Ошибка печати: " + ex.Message, EventLogEntryType.Error, 1, 1);

            }
            myLog.WriteEntry("Конец печати printMessage");

        }

        void Connect(String server, String message)
        {
            Int32 port = 12001;
            if (!EventLog.SourceExists("ServiceGleb1"))
            {
                //An event log source should not be created and immediately used.
                //There is a latency time to enable the source, it should be created
                //prior to executing the application that uses the source.
                //Execute this sample a second time to use the new source.
                EventLog.CreateEventSource("Печать заявок", "ServiceGleb1");
                // The source is created.  Exit the application to allow it to be registered.
                //              return;
            }
            EventLog myLog = new EventLog();
            myLog.Source = "Печать заявок";
            myLog.WriteEntry("Начало работы клиента.", EventLogEntryType.SuccessAudit, 1, 2);
            try
            {
                // Create a TcpClient. Note, for this client to work you need to have a TcpServer connected to the same address as specified by the server, port combination.
                //        PrintServerLink.c
                //        .ActiveForm.SelectServerName
                //        Form f=PrintServerLink.ActiveForm.FindForm();
                //        f.Select();
                TcpClient client = null;
                //        NetworkStream stream = null;
                Byte[] data = null;
                if (ServerName.Text != "")
                    if (PortName.Text != "")
                        if (SendMessage.Text != "")
                        {
                            client = new TcpClient(ServerName.Text, Convert.ToInt32(PortName.Text));
                            //                client = new TcpClient("Win2016", 12001);
                            // Translate the passed message into ASCII and store it as a Byte array.
                            //            data = System.Text.Encoding.Unicode.GetBytes(message);
                            data = System.Text.Encoding.UTF8.GetBytes(SendMessage.Text);
                            // Get a client stream for reading and writing. Stream stream = client.GetStream();
                            stream = client.GetStream();
                            // Send the message to the connected TcpServer. 
                            stream.Write(data, 0, data.Length);
                            myLog.WriteEntry("Послано сообщение ." + SendMessage.Text, EventLogEntryType.SuccessAudit, 1, 2);
                            //            myLog.WriteEntry("Послано сообщение ." + message, EventLogEntryType.SuccessAudit, 1, 2);
                            // Receive the TcpServer.response. Buffer to store the response bytes.
                            data = new Byte[1024];
                            // String to store the response ASCII representation.
                            String responseData = String.Empty;
                            // Read the first batch of the TcpServer response bytes.
                            Int32 bytes = 0;
                            try
                            {
                                bytes = stream.Read(data, 0, data.Length);
                            }
                            catch
                            {
                            }
                            responseData = System.Text.Encoding.UTF8.GetString(data, 0, bytes);
                            myLog.WriteEntry("Принято сообщение ." + responseData, EventLogEntryType.SuccessAudit, 1, 2);
                            printMessage(stream);
                            ReceiveMessage.Text = responseData;
                            // Close everything.
                            stream.Flush();
                            stream.Dispose();
                            stream.Close();
                            client.Close();
                        }
                        else MessageBox.Show("Введите отправляемое сообщение");
                    else MessageBox.Show("Введите номер порта сервера печати");
                else
                    MessageBox.Show("Введите имя сервера");
            }
            catch (ArgumentNullException e)
            {
                myLog.WriteEntry("Исключение аргумента NULL " + e.Message, EventLogEntryType.SuccessAudit, 1, 2);
            }
            catch (SocketException e)
            {
                myLog.WriteEntry("Исключение сокета " + e.Message, EventLogEntryType.SuccessAudit, 1, 2);
                MessageBox.Show("Исключение сокета " + e.Message, e.Message);
            }
            myLog.WriteEntry("Можно отправлять новое сообщение.", EventLogEntryType.SuccessAudit, 1, 2);
        }

        void pd_PrintPage(object sender, PrintPageEventArgs ev)
        {
            float linesPerPage = 0;
            float yPos = 0;
            int count = 0;
            float leftMargin = ev.MarginBounds.Left;
            float topMargin = ev.MarginBounds.Top;
            string line = null;
            // Calculate the number of lines per page.
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

        void button1_Click(object sender, EventArgs e)
        {
            //      NetworkStream str1=new NetworkStream();
            printMessage(stream);
        }

        private void button2_Click(object sender, EventArgs e) // RestartService
        {
            scServices = ServiceController.GetServices();
            foreach (ServiceController scTemp in scServices)
            {
                if (scTemp.ServiceName == "ServiceGleb1")
                {
                    // Display properties for the Simple Service sample from the ServiceBase example.
                    ServiceController sc = new ServiceController("ServiceGleb1");
                    sc.Stop();
                    while (sc.Status != ServiceControllerStatus.Stopped) { Thread.Sleep(1000); sc.Refresh(); }
                    //Console.WriteLine("Status = " + sc.Status);Console.WriteLine("Can Pause and Continue = " + sc.CanPauseAndContinue);
                    //Console.WriteLine("Can ShutDown = " + sc.CanShutdown);Console.WriteLine("Can Stop = " + sc.CanStop);
                    if (sc.Status == ServiceControllerStatus.Stopped)
                    {
                        sc.Start();
                        while (sc.Status == ServiceControllerStatus.Stopped) { Thread.Sleep(1000); sc.Refresh(); }
                    }
                    //          sc.ExecuteCommand((int)SimpleServiceCustomCommands.StopWorker);
                    //          sc.ExecuteCommand((int)SimpleServiceCustomCommands.RestartWorker);
                    //          sc.Pause();
                    //          while (sc.Status != ServiceControllerStatus.Paused) { Thread.Sleep(1000); sc.Refresh(); }
                    //          Console.WriteLine("Status = " + sc.Status);
                    //          sc.Continue();
                }
            }
        }
        private void StopService_Click(object sender, EventArgs e)
        {
            scServices = ServiceController.GetServices();
            foreach (ServiceController scTemp in scServices)
            {
                if (scTemp.ServiceName == "ServiceGleb1")
                {
                    // Display properties for the Simple Service sample from the ServiceBase example.
                    ServiceController sc = new ServiceController("ServiceGleb1");
                    //Console.WriteLine("Status = " + sc.Status);Console.WriteLine("Can Pause and Continue = " + sc.CanPauseAndContinue);
                    //Console.WriteLine("Can ShutDown = " + sc.CanShutdown);Console.WriteLine("Can Stop = " + sc.CanStop);
                    if (sc.Status == ServiceControllerStatus.Running)
                    {
                        sc.Stop();
                        while (sc.Status != ServiceControllerStatus.Stopped) { Thread.Sleep(1000); sc.Refresh(); }
                    }
                }
            }
        }

        private void StartService_Click(object sender, EventArgs e)
        {
            scServices = ServiceController.GetServices();
            foreach (ServiceController scTemp in scServices)
            {
                if (scTemp.ServiceName == "ServiceGleb1")
                {
                    // Display properties for the Simple Service sample from the ServiceBase example.
                    ServiceController sc = new ServiceController("ServiceGleb1");
                    //Console.WriteLine("Status = " + sc.Status);Console.WriteLine("Can Pause and Continue = " + sc.CanPauseAndContinue);
                    //Console.WriteLine("Can ShutDown = " + sc.CanShutdown);Console.WriteLine("Can Stop = " + sc.CanStop);
                    if (sc.Status == ServiceControllerStatus.Stopped)
                    {
                        sc.Start();
                        while (sc.Status == ServiceControllerStatus.Stopped) { Thread.Sleep(1000); sc.Refresh(); }
                    }
                    /*
                              // Issue custom commands to the service enum SimpleServiceCustomCommands 
                              //    { StopWorker = 128, RestartWorker, CheckWorker };
                              sc.ExecuteCommand((int)SimpleServiceCustomCommands.StopWorker);
                              sc.ExecuteCommand((int)SimpleServiceCustomCommands.RestartWorker);
                              sc.Pause();
                              while (sc.Status != ServiceControllerStatus.Paused) { Thread.Sleep(1000); sc.Refresh(); }
                              Console.WriteLine("Status = " + sc.Status);
                              sc.Continue();
                              while (sc.Status == ServiceControllerStatus.Paused) { Thread.Sleep(1000); sc.Refresh(); }
                              Console.WriteLine("Status = " + sc.Status);
                              sc.Stop();
                              while (sc.Status != ServiceControllerStatus.Stopped) { Thread.Sleep(1000); sc.Refresh(); }
                              Console.WriteLine("Status = " + sc.Status);
                              String[] argArray = new string[] { "ServiceController arg1", "ServiceController arg2" };
                              sc.Start(argArray);
                              while (sc.Status == ServiceControllerStatus.Stopped) { Thread.Sleep(1000); sc.Refresh(); }
                              Console.WriteLine("Status = " + sc.Status);
                              // Display the event log entries for the custom commands and the start arguments.
                              EventLog el = new EventLog("Application");
                              EventLogEntryCollection elec = el.Entries;
                              foreach (EventLogEntry ele in elec)
                              {
                                if (ele.Source.IndexOf("SimpleService.OnCustomCommand") >= 0 | ele.Source.IndexOf("SimpleService.Arguments") >= 0)
                                  Console.WriteLine(ele.Message);
                              }
                    */
                }
            }
        }
    }
}
