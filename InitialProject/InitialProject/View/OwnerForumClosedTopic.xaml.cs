using InitialProject.DTO;
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
using System.Windows.Shapes;

namespace InitialProject.View
{
    /// <summary>
    /// Interaction logic for OwnerForumClosedTopic.xaml
    /// </summary>
    public partial class OwnerForumClosedTopic : Window
    {
        public OwnerForumClosedTopic()
        {
            InitializeComponent();
        }

        public OwnerForumClosedTopic(ShowOwnerForumsDTO showOwnerForumsDTO)
        {
            InitializeComponent();
        }
    }
}
