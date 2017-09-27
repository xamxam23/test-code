using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Test_MultiChoice.Model;
using Newtonsoft.Json;
namespace Test_MultiChoice.Data
{
    class GetAnimalsImplementation : GetAnimalsInterface
    {
        Dispatcher Dispatcher;

        public GetAnimalsImplementation(Dispatcher dispatcher)
        {
            Dispatcher = dispatcher;
        }

        public void GetAnimalsCount(ResponseHandler<GetAnimalsCountResponse> handler)
        {
            Execute<GetAnimalsCountResponse>("GetAnimalsCount", handler);
        }

        public void GetAnimalsCountPerGroup(ResponseHandler<GetAnimalsCountPerGroupResponse> handler)
        {
            Execute<GetAnimalsCountPerGroupResponse>("GetAnimalsCountPerGroup", handler);
        }

        public void GetAnimals(ResponseHandler<GetAnimalsResponse> handler)
        {
            Execute<GetAnimalsResponse>("GetAnimals", handler);
        }

        private void Execute<T>(string cmd, ResponseHandler<T> handle)
        {
            Task.Factory.StartNew((Action)(() =>
            {
                RestClient Client = new RestClient(cmd);
                T data = Client.Execute<T>();
                if (data != null && handle != null)
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        handle(data);
                    }));
            }));
        }
    }

    class SetAnimalsCountImplementation : SetAnimalsInfo
    {
        Dispatcher Dispatcher;

        public SetAnimalsCountImplementation(Dispatcher dispatcher)
        {
            Dispatcher = dispatcher;
        }
        Random random = new Random();
        public void CreateAnimals(ResponseHandler<BaseResponse> handle)
        {
            Console.WriteLine("[ CreateAnimals ]");
            Task.Factory.StartNew((Action)(() =>
            {
                List<Animal> list = new List<Animal>();
                try
                {
                    string[] lines = File.ReadAllLines(@"data.txt", Encoding.UTF8);
                    for (int i = 0; i < 200; i++)
                    {
                        string name = lines[random.Next(lines.Length)];
                        int count = random.Next(100);
                        // y,x  |  y in (-32, -24) , x in (22, 30) , y=lat, x=long
                        double x = 17.5 + 13 * random.NextDouble();
                        double y = -(22 + 10 * random.NextDouble());
                        int groupid = 1 + random.Next(5);
                        list.Add(new Animal() { Name = name, Longitude = x, Latitude = y, Group_ID = groupid });
                    }
                    string data = JsonConvert.SerializeObject(list);

                    RestClient Client = new RestClient("CreateAnimals").Put("data", data);
                    BaseResponse response = Client.Execute<BaseResponse>();
                    if (data != null && handle != null)
                        Dispatcher.BeginInvoke((Action)(() =>
                        {
                            handle(response);
                        }));
                }
                catch (IOException e)
                {
                    Console.WriteLine(e);
                }
            }));
        }
        static Random rand = new Random();
        public void UpdateLocations(List<Animal> oldList, ResponseHandler<BaseResponse> handler)
        {
            Console.WriteLine("[ UpdateLocations ]");
            Task.Factory.StartNew((Action)(() =>
            {
                List<Animal> list = new List<Animal>();
                for (int i = 0; i < 20; i++)
                    list.Add(oldList[rand.Next(oldList.Count)].cloneAndMove());

                string data = JsonConvert.SerializeObject(list);
                RestClient Client = new RestClient("UpdateLocations").Put("data", data);
                BaseResponse response = Client.Execute<BaseResponse>();
                if (data != null && handler != null)
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        handler(response);
                    }));
            }));
        }
    }
}
