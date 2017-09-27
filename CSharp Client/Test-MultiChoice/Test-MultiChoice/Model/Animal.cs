
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Test_MultiChoice.Model
{
    public class Animal : INotifyPropertyChanged
    {
        string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    RaisePropertyChanged("Name");
                }
            }
        }
        public int Animal_ID { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int Group_ID { get; set; }

        double[] _Location = new double[] { 0, 0 };
        static Random rand = new Random();
        static double sign()
        {
            return (rand.Next(2) == 1 ? -1 : 1);
        }
        public Animal cloneAndMove()
        {
            double x = (rand.NextDouble() / 2 - 0.5) / 10;
      
            return new Animal()
            {
                Name = this.Name,
                Animal_ID = this.Animal_ID,
                Longitude = this.Longitude + sign()*rand.NextDouble() / 10,
                Latitude = this.Latitude + sign()*rand.NextDouble() / 10,
                Group_ID = this.Group_ID
            };
        }
        public double[] Location
        {
            get
            {
                return _Location;
            }
            set
            {
                if (_Location != value)
                {
                    _Location = value;
                    RaisePropertyChanged("Location");
                }
            }
        }

        void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null) { PropertyChanged(this, new PropertyChangedEventArgs(property)); }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
