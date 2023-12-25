using System.Net.Sockets;
using System.Text.Json;
using System.IO.Pipes;
using IPCTest.Classes.VRCX;

namespace IPCTest {
    internal class Program {
        static async Task Main(string[] args) {
            var ipcServer = new IPCServer();
            ipcServer.Init();
            var ipcClient = new IPCClient(ipcServer: IPCServer.ipcServer);
            ipcClient.SendShowEvent();
            ipcClient.SendLaunchCommand("test");
            ipcClient.BeginRead();
            // var currentUser = await ipcClient.CallMethodAsync<Classes.IPC.User>("getCurrentUser");
        }
    }
}
