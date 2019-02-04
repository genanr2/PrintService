namespace WindowsFormsApplication1
{
  partial class PrintServerLink
  {
    /// <summary>
    /// Требуется переменная конструктора.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Освободить все используемые ресурсы.
    /// </summary>
    /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Код, автоматически созданный конструктором форм Windows

    /// <summary>
    /// Обязательный метод для поддержки конструктора - не изменяйте
    /// содержимое данного метода при помощи редактора кода.
    /// </summary>
    private void InitializeComponent()
    {
      this.SendButton = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.ServerName = new System.Windows.Forms.TextBox();
      this.PortName = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.SendMessage = new System.Windows.Forms.TextBox();
      this.label3 = new System.Windows.Forms.Label();
      this.ReceiveMessage = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.button1 = new System.Windows.Forms.Button();
      this.RestartService = new System.Windows.Forms.Button();
      this.StartService = new System.Windows.Forms.Button();
      this.StopService = new System.Windows.Forms.Button();
      this.SuspendLayout();
      // 
      // SendButton
      // 
      this.SendButton.Location = new System.Drawing.Point(205, 263);
      this.SendButton.Name = "SendButton";
      this.SendButton.Size = new System.Drawing.Size(132, 23);
      this.SendButton.TabIndex = 0;
      this.SendButton.Text = "Отправить сообщение";
      this.SendButton.UseVisualStyleBackColor = true;
      this.SendButton.Click += new System.EventHandler(this.SendButton_Click);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(71, 9);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(74, 13);
      this.label1.TabIndex = 1;
      this.label1.Text = "Имя сервера";
      // 
      // ServerName
      // 
      this.ServerName.Location = new System.Drawing.Point(12, 25);
      this.ServerName.Name = "ServerName";
      this.ServerName.Size = new System.Drawing.Size(207, 20);
      this.ServerName.TabIndex = 2;
      // 
      // PortName
      // 
      this.PortName.Location = new System.Drawing.Point(241, 25);
      this.PortName.Name = "PortName";
      this.PortName.Size = new System.Drawing.Size(97, 20);
      this.PortName.TabIndex = 3;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(259, 9);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(32, 13);
      this.label2.TabIndex = 4;
      this.label2.Text = "Порт";
      // 
      // SendMessage
      // 
      this.SendMessage.AcceptsReturn = true;
      this.SendMessage.AcceptsTab = true;
      this.SendMessage.Location = new System.Drawing.Point(12, 78);
      this.SendMessage.Multiline = true;
      this.SendMessage.Name = "SendMessage";
      this.SendMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.SendMessage.Size = new System.Drawing.Size(326, 66);
      this.SendMessage.TabIndex = 5;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(110, 62);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(142, 13);
      this.label3.TabIndex = 6;
      this.label3.Text = "Отправляемое сообщение";
      // 
      // ReceiveMessage
      // 
      this.ReceiveMessage.AcceptsReturn = true;
      this.ReceiveMessage.AcceptsTab = true;
      this.ReceiveMessage.Location = new System.Drawing.Point(12, 168);
      this.ReceiveMessage.Multiline = true;
      this.ReceiveMessage.Name = "ReceiveMessage";
      this.ReceiveMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.ReceiveMessage.Size = new System.Drawing.Size(326, 89);
      this.ReceiveMessage.TabIndex = 7;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(154, 152);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(37, 13);
      this.label4.TabIndex = 8;
      this.label4.Text = "Ответ";
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(142, 263);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(57, 23);
      this.button1.TabIndex = 9;
      this.button1.Text = "Печать";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // RestartService
      // 
      this.RestartService.Location = new System.Drawing.Point(12, 263);
      this.RestartService.Name = "RestartService";
      this.RestartService.Size = new System.Drawing.Size(124, 23);
      this.RestartService.TabIndex = 10;
      this.RestartService.Text = "Перезапуск службы";
      this.RestartService.UseVisualStyleBackColor = true;
      this.RestartService.Click += new System.EventHandler(this.button2_Click);
      // 
      // StartService
      // 
      this.StartService.Location = new System.Drawing.Point(142, 292);
      this.StartService.Name = "StartService";
      this.StartService.Size = new System.Drawing.Size(124, 23);
      this.StartService.TabIndex = 11;
      this.StartService.Text = "Запуск службы";
      this.StartService.UseVisualStyleBackColor = true;
      this.StartService.Click += new System.EventHandler(this.StartService_Click);
      // 
      // StopService
      // 
      this.StopService.Location = new System.Drawing.Point(12, 292);
      this.StopService.Name = "StopService";
      this.StopService.Size = new System.Drawing.Size(124, 23);
      this.StopService.TabIndex = 12;
      this.StopService.Text = "Остановка службы";
      this.StopService.UseVisualStyleBackColor = true;
      this.StopService.Click += new System.EventHandler(this.StopService_Click);
      // 
      // PrintServerLink
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(386, 345);
      this.Controls.Add(this.StopService);
      this.Controls.Add(this.StartService);
      this.Controls.Add(this.RestartService);
      this.Controls.Add(this.button1);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.ReceiveMessage);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.SendMessage);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.PortName);
      this.Controls.Add(this.ServerName);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.SendButton);
      this.Name = "PrintServerLink";
      this.Text = "Связь с сервером печати";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button SendButton;
    private System.Windows.Forms.Label label1;
    public System.Windows.Forms.TextBox ServerName;
    public System.Windows.Forms.TextBox PortName;
    private System.Windows.Forms.Label label2;
    public System.Windows.Forms.TextBox SendMessage;
    private System.Windows.Forms.Label label3;
    public System.Windows.Forms.TextBox ReceiveMessage;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button RestartService;
    private System.Windows.Forms.Button StartService;
    private System.Windows.Forms.Button StopService;
  }
}

