using Avalonia.Threading;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WatsonTcp;

namespace Remore.Client.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        [Reactive]
        public string Log { get; set; }


        public MainWindowViewModel()
        {
            Connect();
        }

        public void Logger(string message)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                Log += message;
            });
        }

        //TODO: Move all code into separate class
        private async Task Connect()
        {
            WatsonTcpClient client = new WatsonTcpClient("127.0.0.1", 9000);
            client.Events.MessageReceived += MessageReceived;
            client.Callbacks.SyncRequestReceived = SyncRequestReceived;
            client.Connect();
        }

        void MessageReceived(object sender, MessageReceivedEventArgs args)
        {
            Console.WriteLine("Message from server: " + Encoding.UTF8.GetString(args.Data));
        }

        SyncResponse SyncRequestReceived(SyncRequest req)
        {
            return new SyncResponse(req, "{\"PacketId\":\"AdditionalInformationResponse\",\"Name\":\"123\",\"ClientVersion\":\"222\"}");
        }
    }
}