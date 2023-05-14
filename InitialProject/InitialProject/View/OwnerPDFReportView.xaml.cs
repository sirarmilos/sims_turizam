using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using System.Windows.Xps.Packaging;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for OwnerPDFReportView.xaml
    /// </summary>
    public partial class OwnerPDFReportView : Window
    {
        public OwnerPDFReportView()
        {
            InitializeComponent();

            DataContext = this;

            pdfWebViewer.Navigate(new Uri("about:blank"));
            // pdfWebViewer.Navigate("../Resources/Images/pdf_proba.pdf");
        }
    }
}
