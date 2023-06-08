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

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for WizardFooter.xaml
    /// </summary>
    public partial class WizardFooter : UserControl
    {
        public WizardFooter()
        {
            InitializeComponent();
        }

        public event RoutedEventHandler Back;
        public event RoutedEventHandler Next;
        public event RoutedEventHandler Cancel;

        public bool IsBackButtonEnabled
        {
            get { return BackButton.IsEnabled; }
            set { BackButton.IsEnabled = value; }
        }

        public bool IsNextButtonEnabled
        {
            get { return NextButton.IsEnabled; }
            set { NextButton.IsEnabled = value; }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Back?.Invoke(this, e);
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            Next?.Invoke(this, e);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this)?.Close();
        }
    }
}
