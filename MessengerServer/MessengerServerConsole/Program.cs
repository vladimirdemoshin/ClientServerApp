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

namespace ServerConsole
{
    class Program
    {
        static int port = 8005; // порт для приема входящих запросов
        public static byte REGISTRATION_CODE = 0;
        public static byte AUTHENTICATION_CODE = 1;
        public static byte MESSAGE_CODE = 2;

        public static byte REGISTRATION_DONE_CODE = 1;
        public static byte REGISTRATION_FAILED_CODE = 0;

        public static byte AUTHENTICATION_DONE_CODE = 1;
        public static byte AUTHENTICATION_FAILED_CODE = 0;

        public static byte MESSAGE_DONE_CODE = 1;
        public static byte MESSAGE_FAILED_CODE = 0;

        public static BigInteger MAX_PRIME_NUMBER = BigInteger.Parse("1000000000000000000000");

        public static ServerDatabaseEntities db = new ServerDatabaseEntities();

        static private String ComputeHash(String value)
        {
            String hash = "";
            byte[] hashByteValue = new SHA1CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(value));
            for (int i = 0; i < hashByteValue.Length; i++) hash += hashByteValue[i];
            return hash;
        }
        static void Main(string[] args)
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);  // получаем адреса для запуска сокета
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); // создаем сокет
            try
            {
                listenSocket.Bind(ipPoint); //связываем сокет с локальной точкой, по которой будем принимать данные
                listenSocket.Listen(10); //начинаем прослушивание
                Console.WriteLine("Server is on. Waiting for connection...");
                while (true)
                {
                    Socket handler = listenSocket.Accept();
                    BinaryReader br = new BinaryReader(new NetworkStream(handler));
                    byte OPERATION_CODE = br.ReadByte(); 
                    if (OPERATION_CODE == REGISTRATION_CODE)
                    {
                        String login = br.ReadString();
                        String hashPassword = br.ReadString();
                        String output = "Connection: Code of operation: " + OPERATION_CODE + "; login: " + login;
                        Console.WriteLine(output);
                        byte answer = REGISTRATION_DONE_CODE;
                        foreach (User user in db.User)
                            if (user.login == login) answer = REGISTRATION_FAILED_CODE; //отправляем ответ, что логин занят, регистрация не удалась
                        if (answer == REGISTRATION_DONE_CODE) //если логин не занят, то регистрируем
                        {
                            db.User.Add(new User { login = login, password = hashPassword });
                            db.SaveChanges();
                        }
                        handler.Send(new byte[] { answer });
                        BinaryWriter bw = new BinaryWriter(new NetworkStream(handler), Encoding.UTF8);
                        bw.Write(answer);
                        handler.Shutdown(SocketShutdown.Both);
                        handler.Close();
                    }
                    else if(OPERATION_CODE == AUTHENTICATION_CODE)
                    {
                        String login = br.ReadString();
                        //String hashPassword = br.ReadString();
                        String output = "Connection: Code of operation: " + OPERATION_CODE + "; login: " + login;
                        Console.WriteLine(output);
                        BinaryWriter bw = new BinaryWriter(new NetworkStream(handler), Encoding.UTF8);
                        byte answer = AUTHENTICATION_FAILED_CODE;
                        foreach (User user in db.User)
                            if (user.login == login) answer = AUTHENTICATION_DONE_CODE; //если есть пароль, то проверить пароль, иначе вернуть ответ, что не удалось войти
                        if (answer == AUTHENTICATION_DONE_CODE) //если логин занят, проверяем хеш пароля
                        {
                            User user = db.User.Find(login);
                            String hashPasswordFromDB = user.password;
                            BigInteger responseNumber = DiffieHellman.RandomPrimeBigInteger(MAX_PRIME_NUMBER);
                            bw.Write(responseNumber.ToString());
                            String hashInvokeResponseServer = ComputeHash(responseNumber + hashPasswordFromDB);
                            String hashInvokeResponse = br.ReadString();
                            if (hashInvokeResponse != hashInvokeResponseServer) answer = AUTHENTICATION_FAILED_CODE;
                        }
                        handler.Send(new byte[] { answer });
                        handler.Shutdown(SocketShutdown.Both);
                        handler.Close();
                    }
                    else //if(OPERATION_CODE == MESSAGE_CODE)
                    {
                        String login = br.ReadString();
                        BigInteger n = DiffieHellman.RandomPrimeBigInteger(MAX_PRIME_NUMBER);
                        BigInteger q = DiffieHellman.RandomPrimeBigInteger(MAX_PRIME_NUMBER);
                        BinaryWriter bw = new BinaryWriter(new NetworkStream(handler), Encoding.UTF8);
                        bw.Write(n.ToString());
                        bw.Write(q.ToString());
                        BigInteger x = DiffieHellman.RandomPrimeBigInteger(n);
                        BigInteger M = BigInteger.ModPow(q,x,n);
                        bw.Write(M.ToString());
                        BigInteger K = BigInteger.Parse(br.ReadString());
                        BigInteger key = BigInteger.ModPow(K,x,n);
                        String encryptedMessage = br.ReadString();
                        String message = DiffieHellman.decryptRS4(key,encryptedMessage);
                        byte answer = MESSAGE_DONE_CODE;
                        Console.WriteLine("User " + login + " has send a message:");
                        Console.WriteLine(message);
                        handler.Send(new byte[] { answer });
                        bw.Write(answer);
                        handler.Shutdown(SocketShutdown.Both); 
                        handler.Close();
                    }    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
