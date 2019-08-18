using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Media;
using Telerik.Windows.Controls.ChartView;
using PhysModelLibrary;
using System.Threading.Tasks;

namespace PhysModelGUI.Graphics
{
    public partial class LinearGraph : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public List<DataPoint> data = new List<DataPoint>();
        public List<DataPoint> AsyncData1 { get; set; } = new List<DataPoint>();

        private SolidColorBrush series1Color = new SolidColorBrush(Colors.Red);
        public SolidColorBrush Series1Color
        {
            get { return series1Color; }
            set { series1Color = value; OnPropertyChanged(); }
        }

        private string series1Legend = "left ventricle";
        public string Series1Legend
        {
            get { return series1Legend; }
            set { series1Legend = value; OnPropertyChanged(); }
        }

        private int series1StrokeThickness = 1;
        public int Series1StrokeThickness
        {
            get { return series1StrokeThickness; }
            set { series1StrokeThickness = value; OnPropertyChanged(); }
        }

        private double _maxY = 250;
        public double MaxY
        {
            get => _maxY;
            set { _maxY = value; OnPropertyChanged(); }
        }

        private double _minY = 0;
        public double MinY
        {
            get => _minY;
            set { _minY = value; OnPropertyChanged(); }
        }

        private double _gridYStep = 10;
        public double GridYStep
        {
            get => _gridYStep;
            set { _gridYStep = value; OnPropertyChanged(); }
        }

        private bool _showYLabels = true;
        public bool ShowYLabels
        {
            get => _showYLabels;
            set { _showYLabels = value; OnPropertyChanged(); }
        }

        private double _minX = 0;
        public double MinX
        {
            get => _minX;
            set { _minX = value; OnPropertyChanged(); }
        }

        private double _maxX = 300;
        public double MaxX
        {
            get => _maxX;
            set { _maxX = value; OnPropertyChanged(); }
        }
        private double _gridXStep = 10;
        public double GridXStep
        {
            get => _gridXStep;
            set { _gridXStep = value; OnPropertyChanged(); }
        }
        private bool _showXLabels = true;
        public bool ShowXLabels
        {
            get => _showXLabels;
            set { _showXLabels = value; OnPropertyChanged(); }
        }

        public int BufferSize { get; set; } = 200;           // determine the buffer size off 200 datapoints

        public LinearGraph()
        {
            InitializeComponent();

            DataContext = this;

            dataSeries1.XValueBinding = new PropertyNameDataPointBinding() { PropertyName = "Y" };
            dataSeries1.YValueBinding = new PropertyNameDataPointBinding() { PropertyName = "X" };

        }

        public void InitGraph(int buffer)
        {
            BufferSize = buffer;
        }
        public async Task DrawGraph()
        {

            AsyncData1 = new List<DataPoint>(data);
            dataSeries1.ItemsSource = AsyncData1;

            AsyncData1 = null;

        }

        public void UpdateDataBlock(List<DataPoint> newBlock)
        {
            data.AddRange(newBlock);

            int bufferOverflow = data.Count - BufferSize;
            if (bufferOverflow > 0)
            {
                data.RemoveRange(0, bufferOverflow);
            }   
        }

        public void UpdateData(double data1, double data2)
        {
            DataPoint newDataPoint = new DataPoint()
            {
                X = data1,
                Y = data2,
            };

            data.Add(newDataPoint);
            if (data.Count > BufferSize)
            {
                data.RemoveAt(0);
            }
        } 
    }
}
