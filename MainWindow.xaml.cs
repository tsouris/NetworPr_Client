using System;
using System.IO;
using System.Net.Sockets;
using System.Windows;

namespace NetworPr_Client
{
    public partial class MainWindow : Window
    {
        private TcpClient client;
        private StreamReader reader;
        private StreamWriter writer;
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Connect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client = new TcpClient();
                await client.ConnectAsync(tbIp.Text, int.Parse(tbPort.Text));

                NetworkStream stream = client.GetStream();
                reader = new StreamReader(stream);
                writer = new StreamWriter(stream) { AutoFlush = true };
                btnGetQuote.IsEnabled = true;
                btnConnect.IsEnabled = false;

                Log("Connected to server.");
            }
            catch (Exception ex)
            {
                Log($"Error: {ex.Message}");
            }
        }

        private async void GetQuote_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await writer.WriteLineAsync("GET_QUOTE");
                string quote = await reader.ReadLineAsync();
                DisplayQuote(quote);
            }
            catch (Exception ex)
            {
                Log($"Error: {ex.Message}");
            }
        }

        private void DisplayQuote(string quote)
        {
            tbReceivedQuotes.Text += $"{quote}\n";
        }

        private void Log(string message)
        {
            tbLog.Text += $"{DateTime.Now} - {message}\n";
        }

        private void DisconnectButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                client?.Close();
                Log("Disconnected from server.");
                btnGetQuote.IsEnabled = false;
                btnConnect.IsEnabled = true;
            }
            catch (Exception ex)
            {
                Log($"Error: {ex.Message}");
            }
        }
    }
}
