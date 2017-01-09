using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Collections.Concurrent;

namespace lab3
{
    class Program
    {
        static void Main(string[] args)
        {
            ConcurrentBag<string> inWidth = new ConcurrentBag<string>();
            ConcurrentBag<string> inHeight = new ConcurrentBag<string>();
            List<Picture> inputPictures = new List<Picture>();
            List<Picture> resizedPictures = new List<Picture>();
            Picture inputPicture = new Picture();
            // получаем адреса для запуска сокета
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8005);
            // создаем сокет
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                // связываем сокет с локальной точкой, по которой будем принимать данные
                listenSocket.Bind(ipPoint);

                // начинаем прослушивание
                listenSocket.Listen(10);

                Console.WriteLine("Сервер запущен. Ожидание подключений...");
                int endl = 29;
                while (endl > 0)
                {
                    Socket handler = listenSocket.Accept();
                    // получаем сообщение
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0; // количество полученных байтов
                    byte[] data = new byte[256]; // буфер для получаемых данных

                    do
                    {
                        bytes = handler.Receive(data);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (handler.Available > 0);
                    String[] mas = builder.ToString().Split(' ');
                    inWidth.Add(mas[0]);
                    inHeight.Add(mas[1]);
                    Console.WriteLine(DateTime.Now.ToShortTimeString() + ":Сервер принимает: Ширина " + mas[0].ToString() + " Высота " + mas[1].ToString());
                    inputPicture.width = Convert.ToInt32(mas[0]); inputPicture.height = Convert.ToInt32(mas[1]);
                    inputPictures.Add(inputPicture);
                    //ресайз
                    Random rand = new Random();
                    Resizer Res = new Resizer(rand.Next(1, 500), rand.Next(1, 500));
                    resizedPictures = Res.resize(inputPictures);
                    // отправляем ответ
                    int picturesCounter = 1;
                    Console.WriteLine("\nОтправляем картинки:");
                    string message;
                    foreach (Picture i in resizedPictures)
                    {
                        message = i.width.ToString() + " " + i.height.ToString();
                        data = Encoding.Unicode.GetBytes(Convert.ToString(message));
                        handler.Send(data);
                        Console.WriteLine("\nКартинка №" + picturesCounter + ", ширина:" + i.width + ", высота:" + i.height);
                        picturesCounter++;
                    }
                    message = "новые картинки отправлены";
                    data = Encoding.Unicode.GetBytes(message);
                    handler.Send(data);
                    // закрываем сокет
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                    endl--;
                }
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
