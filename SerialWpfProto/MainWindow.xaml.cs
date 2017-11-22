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

namespace SerialWpfProto
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private SerialPort port;
        private ComboBoxItem[] comboItems;
        private String outputText;
        private Boolean portOpened;
        private SerialDataReceivedEventHandler receivedHandler;

        public MainWindow()
        {
            receivedHandler = new SerialDataReceivedEventHandler(dataReceived);
            portOpened = false;
            outputText = "";
            port = new SerialPort();
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
            port.DataReceived += receivedHandler;
            portOpened = true;
            openPortButton.IsEnabled = false;
            closePortButton.IsEnabled = true;
            sendButton.IsEnabled = true;
        }

        private void closePortButton_Click(object sender, RoutedEventArgs e)
        {
            port.DataReceived -= receivedHandler;
            portOpened = false;
            port.Close();
            closePortButton.IsEnabled = false;
            openPortButton.IsEnabled = true;
            sendButton.IsEnabled = false;
            clearButton.IsEnabled = true;
        }

        /*TODO: Replace with clock controlled thread*/
        private void dataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (!portOpened) {
                return;
            }
            
            if (port.IsOpen)  
            {
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
                    outputText += strRcv;
                    Dispatcher.BeginInvoke(new Action(delegate
                    {
                        resultTextBlock.Text = outputText;
                    }));
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
            }
            else
            {
                MessageBox.Show("Please open a port", "Error");
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            outputText = "";
            resultTextBlock.Text = "Result";
        }
    }
}
