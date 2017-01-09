using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace lab3_client
{
    class Program
    {
        static void Main(string[] args)
        {
            //List<Picture> resizedPictures = new List<Picture>();
            Random rand = new Random();
            User User1 = new User();
            User1.countPictures = rand.Next(1, 5);
            Console.WriteLine("Генерация картинок:");
            for (int i = 0; i < User1.countPictures; i++)
            {
                User1.generatePicture(rand.Next(1,500), rand.Next(1, 500));
            }
            int picturesCounter = 1;
            foreach (Picture i in User1.picturesList)
            {
                Console.WriteLine("\nКартинка №" + picturesCounter + ", высота:" + i.height + ", ширина:" + i.width);
                picturesCounter++;
            }

            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8005);

            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            // подключаемся к удаленному хосту
            socket.Connect(ipPoint);

            Console.WriteLine("Отправка картинок");
            try
            {
                foreach (Picture i in User1.picturesList)
                {
                    string message = i.width.ToString() + " " + i.height.ToString();
                    byte[] data = Encoding.Unicode.GetBytes(message);
                    socket.Send(data);


                    // получаем ответ
                    data = new byte[256]; // буфер для ответа
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0; // количество полученных байт
                    do
                    {
                        bytes = socket.Receive(data, data.Length, 0);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (socket.Available > 0);
                    Console.WriteLine("ответ сервера: " + builder.ToString());
                }
                
            }
            
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            // закрываем сокет
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();

            Console.Read();
        }
    }
}
