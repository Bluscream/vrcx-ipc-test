using System.IO.Pipes;
using System.Text;
using System.Text.Json;

namespace IPCTest.Classes.VRCX {
    public class IPCClient {
        public static readonly UTF8Encoding noBomEncoding = new UTF8Encoding(false, false);
        public readonly NamedPipeServerStream _ipcServer;
        public readonly byte[] _recvBuffer = new byte[1024 * 8];
        public readonly MemoryStream memoryStream;
        public readonly byte[] packetBuffer = new byte[1024 * 1024];
        public readonly JsonSerializerOptions options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        public string _currentPacket;

        public IPCClient(NamedPipeServerStream ipcServer) {
            memoryStream = new MemoryStream(packetBuffer);
            _ipcServer = ipcServer;
        }

        public void BeginRead() {
            _ipcServer.BeginRead(_recvBuffer, 0, _recvBuffer.Length, OnRead, _ipcServer);
        }

        public void Send(IPCPacket ipcPacket) {
            try {
                memoryStream.Seek(0, SeekOrigin.Begin);
                using (var streamWriter = new StreamWriter(memoryStream, noBomEncoding, 65535, true)) {
                    var serialized = JsonSerializer.Serialize(ipcPacket, options);
                    Console.WriteLine(serialized);
                    streamWriter.Write(serialized);
                    streamWriter.Write((char)0x00);
                    streamWriter.Flush();
                }

                var length = (int)memoryStream.Position;
                _ipcServer?.BeginWrite(packetBuffer, 0, length, OnSend, null);
            } catch {
                IPCServer.Clients.Remove(this);
            }
        }


        public void OnRead(IAsyncResult asyncResult) {
            try {
                var bytesRead = _ipcServer.EndRead(asyncResult);

                if (bytesRead <= 0) {
                    IPCServer.Clients.Remove(this);
                    _ipcServer.Close();
                    return;
                }

                _currentPacket += Encoding.UTF8.GetString(_recvBuffer, 0, bytesRead);

                if (_currentPacket[_currentPacket.Length - 1] == (char)0x00) {
                    var packets = _currentPacket.Split((char)0x00);

                    foreach (var packet in packets) {
                        if (string.IsNullOrEmpty(packet))
                            continue;

                        Console.WriteLine(packet.ToString());
                    }

                    _currentPacket = string.Empty;
                }
            } catch (Exception e) {
                Console.WriteLine(e);
            }

            BeginRead();
        }

        public static void OnSend(IAsyncResult asyncResult) {
            var ipcClient = (NamedPipeClientStream)asyncResult.AsyncState;
            ipcClient?.EndWrite(asyncResult);
        }

        public static void Close(IAsyncResult asyncResult) {
            var ipcClient = (NamedPipeClientStream)asyncResult.AsyncState;
            ipcClient?.EndWrite(asyncResult);
            ipcClient?.Close();
        }

        #region Methods
        public void SendShowEvent() {
            var ipcEvent = new IPCPacket {
                Type = "show",
                Data = null // replace null with your data if any
            };
            Send(ipcEvent);
        }

        public void SendHideEvent() {
            var ipcEvent = new IPCPacket {
                Type = "hide",
                Data = null // replace null with your data if any
            };
            Send(ipcEvent);
        }

        public void SendExitEvent() {
            var ipcEvent = new IPCPacket {
                Type = "exit",
                Data = null // replace null with your data if any
            };
            Send(ipcEvent);
        }

        public void SendLaunchCommand(string command) {
            Send(new IPCPacket() {
                Type = "LaunchCommand",
                Data = command
            });
        }
        #endregion Methods
    }
}
