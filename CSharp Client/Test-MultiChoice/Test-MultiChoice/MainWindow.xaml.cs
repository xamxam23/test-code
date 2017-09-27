using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GoogleMapsApi;
using Test_MultiChoice.Data;

using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Threading;
using Test_MultiChoice.Utils;

namespace Test_MultiChoice
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window, IGoogleMapHost, SimulatorContract
    {
        static Random rand = new Random();
        private IGoogleMapWrapper _gMapsWrapper;
        private AnimalMotionSimulator _simulator;
        public MainWindow()
        {
            InitializeComponent();

            // CreateAnimals();
            _gMapsWrapper = GoogleMapWrapper.Create(this);
            _gMapsWrapper.ApiReady += _gMapsWrapper_ApiReady;
            /* _gMapsWrapper.MapClick -= _gMapsWrapper_MapClick;
             _gMapsWrapper.ZoomChanged -= _gMapsWrapper_ZoomChanged;
             _gMapsWrapper.CenterChanged -= _gMapsWrapper_CenterChanged;
             _gMapsWrapper.MapDoubleClick -= _gMapsWrapper_MapDoubleClick;
             _gMapsWrapper.MapRightClick -= _gMapsWrapper_MapRightClick;
             _gMapsWrapper.BoundsChanged -= _gMapsWrapper_BoundsChanged;*/
            _simulator = new AnimalMotionSimulator();
            _simulator.Start(this);
        }

        void _gMapsWrapper_ApiReady()
        {
            _gMapsWrapper.Zoom = 6;
            Task.Delay(2000).ContinueWith(t =>
            {
                GetAnimals();
                GetAnimalsCount();
            });

        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (_simulator != null) _simulator.Stop();
            base.OnClosing(e);
        }

        public void RegisterScriptingObject(IGoogleMapRequired wrapper)
        {
            Browser.ObjectForScripting = wrapper;
        }

        public void SetHostDocumentText(string text)
        {
            Browser.NavigateToString(text);
        }

        public object InvokeScript(string methodName, params object[] parameters)
        {
            try
            {
                return Browser.InvokeScript(methodName, parameters);
            }
            catch (Exception)
            {
            }
            return null;

        }

        public bool HandleException(string message, string url, string line)
        {
            return true;
        }

        private void ButtonClick_UpdateAnimals(object sender, RoutedEventArgs e)
        {
            GetAnimals();
        }

        private void CreateAnimals()
        {
            SetAnimalsCountImplementation interactor = new SetAnimalsCountImplementation(Dispatcher);
            interactor.CreateAnimals((BaseResponse response) =>
            {
                GetAnimals();
                GetAnimalsCount();

            }); // insert test data
        }

        private void GetAnimalsCount()
        {
            Test_MultiChoice.Data.GetAnimalsInterface loader = new Test_MultiChoice.Data.GetAnimalsImplementation(Dispatcher);
            loader.GetAnimalsCount((GetAnimalsCountResponse response) =>
            {
                if (response != null)
                {// TODO MVVM
                    List<Model.AnimalCount> data = response.Data;
                    lock (ListView_AnimalCounts)
                    {
                        ListView_AnimalCounts.ItemsSource = null;
                        ListView_AnimalCounts.ItemsSource = data;
                    }
                }
            });
        }

        private void GetAnimalsCountPerGroup()
        {
            ButtonGraph.Visibility = Visibility.Collapsed;
            LabelGraph.Visibility = Visibility.Visible;

            Test_MultiChoice.Data.GetAnimalsInterface loader = new Test_MultiChoice.Data.GetAnimalsImplementation(Dispatcher);
            loader.GetAnimalsCountPerGroup((GetAnimalsCountPerGroupResponse response) =>
            {
                if (response != null)
                {// TODO MVVM
                    List<Model.AnimalCountPerGroup> data = response.Data;
                    new ExcelTool().CreateExcelGraph_AnimalsCountPerGroup(data);
                }
                ButtonGraph.Visibility = Visibility.Visible;
                LabelGraph.Visibility = Visibility.Collapsed;
            });
        }

        private void UpdateLocations(List<Model.Animal> animals)
        {
            Test_MultiChoice.Data.SetAnimalsInfo loader = new Test_MultiChoice.Data.SetAnimalsCountImplementation(Dispatcher);
            loader.UpdateLocations(animals, (BaseResponse response) =>
            {
                if (response != null)
                {
                    Console.WriteLine(response.Message);
                }
            });
        }

        public void GetAnimals()
        {
            Test_MultiChoice.Data.GetAnimalsInterface loader = new Test_MultiChoice.Data.GetAnimalsImplementation(Dispatcher);
            loader.GetAnimals((GetAnimalsResponse response) =>
            {
                if (response != null)
                {
                    List<Model.Animal> animals = new List<Model.Animal>();
                    _gMapsWrapper.Clean();
                    foreach (Model.Animal animal in response.Data)
                    {
                        animals.Add(animal);
                        double x = animal.Longitude;
                        double y = animal.Latitude;
                        AddAnimalMarker(y, x);
                    }
                    UpdateLocations(animals);
                }
            });
        }

        private void AddAnimalMarker(double y, double x)
        {
            try
            {
                if (_gMapsWrapper != null)
                    _gMapsWrapper.AddMarker(new GeographicLocation(y, x), new MarkerOptions(), false);
            }
            catch (NullReferenceException e) { }
        }

        private void ButtonClick_CreateAnimals(object sender, RoutedEventArgs e)
        {
            CreateAnimals();
        }

        private void ButtonClick_Graph(object sender, RoutedEventArgs e)
        {
            GetAnimalsCountPerGroup();
        }

        private void ButtonClick_Graph_Timed(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("To be implemented!");
        }
    }
}