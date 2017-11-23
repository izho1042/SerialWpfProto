using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Ports;
using System.Threading;
using System.Collections;

namespace SerialWpfProto
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private SerialPort port;
        private ComboBoxItem[] comboItems;
        private String receivedText;
        private String displayText;
        private Boolean portOpened;
        //private SerialDataReceivedEventHandler receivedHandler;
        private Thread receiveThread;
        private Boolean tReceiving;
        private ReceiveMode receiveMode;
        private ArrayList displayList;
        private int receiveInterval;

        public MainWindow()
        {
            port = new SerialPort();
            receivedText = "";
            displayText = "";
            portOpened = false;
            //receivedHandler = new SerialDataReceivedEventHandler(dataReceived);
            receiveThread = new Thread(receiveLoop);
            tReceiving = false;
            receiveMode = ReceiveMode.Response;
            displayList = new ArrayList();
            receiveInterval = 300;

            InitializeComponent();

            string[] str = SerialPort.GetPortNames();
            if (str == null)
            {
                MessageBox.Show("This machine doesn't have a serial port", "Error");
                return;
            }
            comboItems = new ComboBoxItem[str.Length];
            for (int i = 0; i < str.Length; i++)
            {
                comboItems[i] = new ComboBoxItem();
                comboItems[i].Content = str[i];
                portCombo.Items.Add(comboItems[i]);
            }
            portCombo.SelectedIndex = 0;

        }

        private void sendButton_Click(object sender, RoutedEventArgs e)
        {
            if (sendInput.Text.Equals("Send Message"))
            {
                return;
            }
            if (!port.IsOpen)
            {
                MessageBox.Show("No open port available. Please setup the port !","Error");
                return;
            }
            string[] inputStr = sendInput.Text.Split(' ');
            Byte[] sendBuffer = new Byte[inputStr.Length];
            for (int i = 0; i < inputStr.Length; i++)
            {
                string temp = inputStr[i];
                inputStr[i] = "0x" + temp;
                try
                {
                    sendBuffer[i] = Convert.ToByte(inputStr[i], 16);
                }
                catch (Exception ee)
                {
                    return;
                }
            }
            port.Write(sendBuffer,0,sendBuffer.Length);
            sendInput.Text = "Send Message";
            if (receiveMode == ReceiveMode.Response)
            {
                dataReceived();
            }
        }

        private void openPortButton_Click(object sender, RoutedEventArgs e)
        {
            clearButton.IsEnabled = false;
            port = new SerialPort();
            port.BaudRate = int.Parse(((ComboBoxItem)baudCombo.SelectedItem).Content.ToString());
            port.PortName = ((ComboBoxItem)portCombo.SelectedItem).Content.ToString();
            port.DataBits = 8;
            try
            {
                port.Open();
            }
            catch (Exception ea) {
                MessageBox.Show(ea.Message,"Error");
                return;
            }
            //port.DataReceived += receivedHandler;
            portOpened = true;
            openPortButton.IsEnabled = false;
            closePortButton.IsEnabled = true;
            sendButton.IsEnabled = true;
            if (receiveMode == ReceiveMode.Continuous) {
                tReceiving = true;
                startThread();
            }
        }

        private void closePortButton_Click(object sender, RoutedEventArgs e)
        {
            //port.DataReceived -= receivedHandler
            if (receiveMode == ReceiveMode.Continuous)
            {
                tReceiving = false;
                receiveThread.Join();
                receiveThread.Abort();
            }
            portOpened = false;
            port.Close();
            closePortButton.IsEnabled = false;
            openPortButton.IsEnabled = true;
            sendButton.IsEnabled = false;
            clearButton.IsEnabled = true;
        }

        private void startThread()
        {
            receiveThread = new Thread(receiveLoop);
            if (!portOpened)
            {
                return;
            }

            if (port.IsOpen)
            {
                receiveThread.Start();
            }
        }

        private void receiveLoop()
        {
            while (tReceiving) {
                Thread.Sleep(receiveInterval);
                dataReceived();
            }
        }

        /* Receive data once */
        private void dataReceived()
        {
            displayText = "";
            try
            {
                Byte[] receivedData = new Byte[port.BytesToRead];
                port.Read(receivedData, 0, receivedData.Length);
                port.DiscardInBuffer();
                string strRcv = null;

                for (int i = 0; i < receivedData.Length; i++)
                {
                    strRcv += receivedData[i].ToString("X2");  //Hexadecimal
                }
                receivedText += strRcv;
                if (receiveMode == ReceiveMode.Continuous)
                {
                    if (displayList.Count >= 5)
                    {
                        displayList.RemoveAt(0);
                    }
                    displayList.Add(strRcv);

                    if (displayList.Count < 1) {
                        return;
                    }

                    for (int i = 0; i < displayList.Count; i++)
                    {
                        displayText += (String)displayList[i];
                    }
                }
                else {
                    displayText = receivedText.Clone().ToString();
                }
                Dispatcher.BeginInvoke(new Action(delegate
                {
                    resultTextBlock.Text = displayText;
                }));
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            displayText = "";
            resultTextBlock.Text = "Result";
        }

        private void responseRadio_Checked(object sender, RoutedEventArgs e)
        {
            receiveMode = ReceiveMode.Response;
            if (continuousRadio == null)
            {
                return;
            }
            continuousRadio.IsChecked = false;
        }

        private void continuousRadio_Checked(object sender, RoutedEventArgs e)
        {
            receiveMode = ReceiveMode.Continuous;
            if (responseRadio == null)
            {
                return;
            }
            responseRadio.IsChecked = false;
        }
    }
}
