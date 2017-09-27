using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Test_MultiChoice.Utils
{
    public interface SimulatorContract
    {
        void GetAnimals();
    }

    public class AnimalMotionSimulator
    {

        static Random rand = new Random();
        private bool running;
        public void Stop()
        {
            running = false;
        }
        public void Start(SimulatorContract simulatorContract)
        {
            running = true;
            new Thread(() =>
            {
                Thread.Sleep(2000);
                while (running)
                {
                    simulatorContract.GetAnimals();
                    Thread.Sleep(1000 + rand.Next(1000));
                }
            }).Start();
        }
    }
}
