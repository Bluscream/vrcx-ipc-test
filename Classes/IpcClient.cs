using System;
using System.IO.Pipes;
using System.Text;

namespace IPCTest.Classes {

    public class IpcClient {
        private NamedPipeClientStream client;

        public IpcClient() {
            client = new NamedPipeClientStream(".", "vrcx-ipc", PipeDirection.InOut, PipeOptions.Asynchronous);
        }

        public void Connect() {
            client.Connect();
        }

        public void Send(string message) {
            if (!client.IsConnected) {
                throw new InvalidOperationException("IPC client is not connected.");
            }

            byte[] buffer = Encoding.UTF8.GetBytes(message);
            client.Write(buffer, 0, buffer.Length);
        }

        public string Receive() {
            if (!client.IsConnected) {
                throw new InvalidOperationException("IPC client is not connected.");
            }

            byte[] buffer = new byte[4096];
            int bytesRead = client.Read(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer, 0, bytesRead);
        }
    }

}
