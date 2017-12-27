using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Security.Cryptography;
using System.Numerics;
using System.Windows.Forms;

namespace Client
{
    static class Connection
    {
        static int port = 8005;
        static string address = "127.0.0.1"; 
        static IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(address), port);

        public static byte REGISTRATION_CODE = 0;
        public static byte AUTHENTICATION_CODE = 1;
        public static byte MESSAGE_CODE = 2;

        public static byte REGISTRATION_DONE_CODE = 1;
        public static byte REGISTRATION_FAILED_CODE = 0;

        public static byte AUTHENTICATION_DONE_CODE = 1;
        public static byte AUTHENTICATION_FAILED_CODE = 0;

        public static byte MESSAGE_DONE_CODE = 1;
        public static byte MESSAGE_FAILED_CODE = 0;
        static private String ComputeHash(String value)
        {
            String hash = "";
            byte[] hashByteValue = new SHA1CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(value));
            for (int i = 0; i < hashByteValue.Length; i++) hash += hashByteValue[i];
            return hash;
        }

        static public byte Registration(String login, String password)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            String hashPassword = ComputeHash(password);
            try
            {
                socket.Connect(ipPoint);
                BinaryWriter bw = new BinaryWriter(new NetworkStream(socket), Encoding.UTF8);
                bw.Write(REGISTRATION_CODE);
                bw.Write(login);
                bw.Write(hashPassword);
                byte[] answer = new byte[1];
                socket.Receive(answer);
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                return answer[0];
            }
            catch (Exception ex) { return REGISTRATION_FAILED_CODE; }
        }

        static public byte Authentication(String login, String password)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            String hashPassword = ComputeHash(password);
            try
            {
                socket.Connect(ipPoint);
                BinaryWriter bw = new BinaryWriter(new NetworkStream(socket), Encoding.UTF8);
                bw.Write(AUTHENTICATION_CODE);
                bw.Write(login);
                BinaryReader br = new BinaryReader(new NetworkStream(socket));
                String responseNumber = br.ReadString();
                String hashInvokeResponse = ComputeHash(responseNumber+hashPassword);
                bw.Write(hashInvokeResponse);
                byte[] answer = new byte[1];
                socket.Receive(answer);
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                return answer[0];
            }
            catch (Exception ex) { return AUTHENTICATION_FAILED_CODE; }
        }

        static public byte SendMessage(String login, String message)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(ipPoint);
                BinaryWriter bw = new BinaryWriter(new NetworkStream(socket), Encoding.UTF8);
                bw.Write(MESSAGE_CODE);
                bw.Write(login);
                BinaryReader br = new BinaryReader(new NetworkStream(socket));
                BigInteger n = BigInteger.Parse(br.ReadString());
                BigInteger q = BigInteger.Parse(br.ReadString());
                BigInteger y = DiffieHellman.RandomPrimeBigInteger(n);
                BigInteger K = BigInteger.ModPow(q, y, n);
                bw.Write(K.ToString());
                BigInteger M = BigInteger.Parse(br.ReadString());
                BigInteger key = BigInteger.ModPow(M, y, n);
                String encryptedMessage = DiffieHellman.encryptRS4(key.ToString(),message);
                bw.Write(encryptedMessage);
                byte[] answer = new byte[1];
                socket.Receive(answer);
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                return answer[0];
            }
            catch (Exception ex) { return MESSAGE_FAILED_CODE; }
        }
    }
}
