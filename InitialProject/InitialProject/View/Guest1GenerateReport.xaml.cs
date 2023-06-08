//using InitialProject.DTO;
//using InitialProject.Model;
//using InitialProject.Repository;
//using InitialProject.Service;
//using iTextSharp.text.pdf;
//using iTextSharp.text;
//using Microsoft.Win32;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using InitialProject.DTO;
using InitialProject.Service;

namespace InitialProject.View
{
    public partial class Guest1GenerateReport : Page
    {
        private readonly ReservationReschedulingRequestService reservationReschedulingRequestService;
        private string accommodationName;
        private string guest1;

        public string Guest1
        {
            get { return guest1; }
            set
            {
                guest1 = value;
            }
        }



        private readonly ReviewService reviewService;

        public List<ShowOwnerReviewsDTO> ShowOwnerReviewsDTOs
        {
            get;
            set;
        }

        public int ReservationsNumber { get; set; }

        public string OwnerUsername { get; set; }

        private string selectedFilePath;
        public List<Guest1PDFReportDTO> Guest1PDFReportDTOs { get; set; }


        private bool notification;
        public bool Notification
        {
            get { return notification; }
            set
            {
                notification = value;
            }
        }

        private void CheckNotification()
        {
            if (Notification)
            {
                NotificationMenuItemImageNotificationBell.Visibility = Visibility.Visible;
                NotificationMenuItemImageRegularBell.Visibility = Visibility.Collapsed;
            }
            else
            {
                NotificationMenuItemImageNotificationBell.Visibility = Visibility.Collapsed;
                NotificationMenuItemImageRegularBell.Visibility = Visibility.Visible;
            }
        }

        public Guest1GenerateReport(string guest1, Page page)
        {
            InitializeComponent();

            Guest1 = guest1;

            DataContext = this;


            reservationReschedulingRequestService = new ReservationReschedulingRequestService(Guest1);

            //SetDefaultValue();

            //Guest1RebookingRequestsDTOs = reservationReschedulingRequestService.FindAllByGuest1Username();

            SetUsernameHeader();

            SetComboBoxes(page);


            reviewService = new ReviewService(Guest1);
            Guest1PDFReportDTOs = reviewService.FindGuest1PDFReportDTOs();
            ReservationsNumber = reviewService.FindNumberOfGuest1Reservations(Guest1);

        }

        //private void SetDefaultValue()
        //{
        //    OwnerPDFReportDTOs = reviewService.FindOwnerPDFReportDTOs(OwnerUsername);
        //    //lvShowGuestReviews.ItemsSource = OwnerPDFReportDTOs;

        //    //labelOwnerUsername.Content = OwnerUsername;
        //    //labelOwnerType.Content = CheckSuperType();
        //    //labelNumberOfAccommodations.Content = OwnerPDFReportDTOs.Count.ToString();
        //    //labelReportDate.Content = DateTime.Now.ToStrin/g("dd/MM/yyyy");
        //    //labelReportTime.Content = DateTime.Now.ToString("HH:mm:ss");
        //}



        private void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF Document (*.pdf)|*.pdf";
            if (saveFileDialog.ShowDialog() == true)
            {
                selectedFilePath = saveFileDialog.FileName;
                FilePathTB.Text = "File path selected: " + selectedFilePath;
                FilePathTB.Visibility = Visibility.Visible;
                ErrorTB.Visibility = Visibility.Collapsed;  
            }
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(selectedFilePath))
            {
                ErrorTB.Text = "Please select a file path before generating the PDF.";
                ErrorTB.Visibility = Visibility.Visible;

                return;
            }

            GeneratePDF(selectedFilePath);
        }


        private void GeneratePDF(string filePath)
        {
            Document document = new Document();
            document.AddAuthor(Guest1);
            document.AddCreationDate();

            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                PdfWriter writer = PdfWriter.GetInstance(document, stream);
                document.Open();

                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance("https://i.ibb.co/WFdc6pB/logo.png");
                logo.ScaleAbsoluteWidth(100);
                logo.ScaleAbsoluteHeight(80);

                logo.Alignment = Element.ALIGN_CENTER;

                document.Add(logo);

                Paragraph title = new Paragraph("Report of Average Owners' Ratings", new Font(Font.FontFamily.HELVETICA, 24, Font.BOLD));
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);

                document.Add(new Paragraph(" ")); 


                Font cellFont = new Font(Font.FontFamily.HELVETICA, 10);

                PdfPTable preTable = new PdfPTable(3);
                preTable.WidthPercentage = 50;
                preTable.SetWidths(new float[] { 2f, 2f, 2f });
                preTable.HorizontalAlignment = Element.ALIGN_LEFT;

                AddPretableHeader(preTable, cellFont);
                AddPretableData(preTable, cellFont);

                document.Add(preTable);

                document.Add(new Paragraph(" ")); 


                PdfPTable table = new PdfPTable(7);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 1f, 1.2f, 1.2f, 1.2f, 2f, 1f, 1f });


                AddTableHeader(table, cellFont);
                AddTableData(table, cellFont);

                document.Add(table);

                PdfContentByte cb = writer.DirectContent;
                cb.BeginText();
                cb.SetFontAndSize(BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 10);
                cb.SetRGBColorFill(0, 0, 0);
                cb.SetTextMatrix(40, 30);
                cb.ShowText(Guest1);
                cb.SetTextMatrix(document.PageSize.Width - 120, 30);
                cb.ShowText(DateTime.Now.ToString("dd/MM/yyyy"));
                cb.EndText();

                document.Close();
            }

            FirstWindow.Visibility = Visibility.Collapsed;
            SecondWindow.Visibility = Visibility.Visible;
        }


        private void AddTableHeader(PdfPTable table, Font font)
        {
            table.AddCell(CreateCell("Owner username", font));
            table.AddCell(CreateCell("Cleanliness average", font));
            table.AddCell(CreateCell("Follow rules average", font));
            table.AddCell(CreateCell("Behavior average", font));
            table.AddCell(CreateCell("Communicativeness average", font));
            table.AddCell(CreateCell("All average", font));
            table.AddCell(CreateCell("Number of reviews", font));
        }

        private void AddTableData(PdfPTable table, Font font)
        {
            foreach (Guest1PDFReportDTO dto in Guest1PDFReportDTOs)
            {
                table.AddCell(CreateCell(dto.OwnerUsername, font));
                table.AddCell(CreateCell((Math.Round(dto.CleanlinessAverage,2)).ToString(), font));
                table.AddCell(CreateCell((Math.Round(dto.FollowRulesAverage)).ToString(), font));
                table.AddCell(CreateCell((Math.Round(dto.BehaviorAverage)).ToString(), font));
                table.AddCell(CreateCell((Math.Round(dto.CommunicativenessAverage)).ToString(), font));
                table.AddCell(CreateCell((Math.Round(dto.AllAverage)).ToString(), font));
                table.AddCell(CreateCell(dto.NumberOfReservations.ToString(), font)); // reviews

            }
        }

        private void AddPretableHeader(PdfPTable table, Font font)
        {
            table.AddCell(CreateCell("Guest1 username: ", font));
            table.AddCell(CreateCell("Type: ", font));
            table.AddCell(CreateCell("Number of reservations: ", font));
        }

        private void AddPretableData(PdfPTable table, Font font)
        {
            table.AddCell(CreateCell(Guest1, font));

            if (CheckSuperType().Equals(""))
                table.AddCell(CreateCell("regular guest", font));
            else
                table.AddCell(CreateCell(CheckSuperType(), font));

            table.AddCell(CreateCell(ReservationsNumber.ToString(), font));
        }


        private PdfPCell CreateCell(string text, Font font)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, font));
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            return cell;
        }




        private void SetUsernameHeader()
        {
            Notification = reservationReschedulingRequestService.Guest1HasNotification();
            CheckNotification();
            usernameAndSuperGuest.Text = $"{Guest1}";
            superGuest.Text = $"{CheckSuperType()}";
        }

        private string CheckSuperType()
        {
            string superType = string.Empty;

            if (reservationReschedulingRequestService.IsSuperGuest(Guest1))
            {
                superType = " (Super guest)";
            }

            return superType;
        }

        void LoadingRowForDgBookingMoveRequests(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private bool comboBoxClicked = false;
        private bool itemClicked = false;

        private void CBPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            comboBoxClicked = true;
        }

        private void CBCreateReviewDropDownClosed(object sender, EventArgs e)
        {
            if (comboBoxClicked && itemClicked)
            {
                ComboBox comboBox = (ComboBox)sender;
                ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;

                if (selectedItem.Content.ToString() == "Create review")
                {
                    GoToCreateReview(sender, null);
                }
                else if (selectedItem.Content.ToString() == "Reviews")
                {
                    GoToShowOwnerReviews(sender, null);
                }
                else if (selectedItem.Content.ToString() == "Requests")
                {
                    GoToGuest1Requests(sender, null);
                }
            }

            comboBoxClicked = false;
            itemClicked = false;
        }

        private void CBSuperGuestDropDownClosed(object sender, EventArgs e)
        {
            if (comboBoxClicked && itemClicked)
            {
                ComboBox comboBox = (ComboBox)sender;
                ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;

                if (selectedItem.Content.ToString() == "Super-guest")
                {
                    GoToSearchAndShowAccommodations(sender, null);
                }
                else if (selectedItem.Content.ToString() == "Logout")
                {
                    GoToLogout(sender, null);
                }
            }

            comboBoxClicked = false;
            itemClicked = false;
        }

        private void CBItemPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            itemClicked = true;
        }

        private void CreateRequest(object sender, RoutedEventArgs e)
        {
            GoToShowReservations(sender, e);
        }

        private void GoToShowOwnerReviews(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ShowOwnerReviews(Guest1, this));
        }

        private void GoToForum(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Guest1Forum(Guest1, this));
        }

        //private void GoToGuest1Start(object sender, RoutedEventArgs e)
        //{
        //    NavigationService?.Navigate(new Guest1Start(Guest1, this));
        //}

        private void GoToSearchAndShowAccommodations(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new SearchAndShowAccommodations(Guest1, this));
        }

        private void GoToShowReservations(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ShowReservations(Guest1, this));
        }

        private void GoToCreateReview(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new CreateReview(Guest1, this));
        }

        private void GoToGuest1Requests(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Guest1Requests(Guest1, this));
        }

        private void GoToShowGuest1Notifications(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new ShowGuest1Notifications(Guest1, this));
        }

        private void GoToAnywhereAnytime(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Guest1AnywhereAnytime(Guest1, this));
        }

        private void GoToLogout(object sender, RoutedEventArgs e)
        {
            Window currentWindow = Window.GetWindow(this);

            LoginForm window = new LoginForm();
            window.Show();
            currentWindow.Close();
        }

        private void SetComboBoxes(Page page)
        {
            if (page is SearchAndShowAccommodations searchAndShowPage)
            {
                var comboBox = searchAndShowPage.CBCreateReview;
                if (comboBox != null)
                {
                    CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
                }

                comboBox = searchAndShowPage.CBSuperGuest;
                if (comboBox != null)
                {
                    CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
                }
            }
            else if (page is AccommodationReservation accommodationReservationPage)
            {
                //var comboBox = accommodationReservationPage.CBCreateReview;
                //if (comboBox != null)
                //{
                //    CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
                //}

                //comboBox = accommodationReservationPage.CBSuperGuest;
                //if (comboBox != null)
                //{
                //    CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
                //}
            }
            else if (page is CreateReservationReschedulingRequest createReschedulingRequestPage)
            {
                var comboBox = createReschedulingRequestPage.CBCreateReview;
                if (comboBox != null)
                {
                    CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
                }

                comboBox = createReschedulingRequestPage.CBSuperGuest;
                if (comboBox != null)
                {
                    CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
                }
            }
            else if (page is CreateReview createReviewPage)
            {
                //var comboBox = createReviewPage.CBCreateReview;
                //if (comboBox != null)
                //{
                //    CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
                //}

                //comboBox = createReviewPage.CBSuperGuest;
                //if (comboBox != null)
                //{
                //    CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
                //}
            }
            else if (page is Guest1RequestPreview guest1RequestPreviewPage)
            {
                var comboBox = guest1RequestPreviewPage.CBCreateReview;
                if (comboBox != null)
                {
                    CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
                }

                comboBox = guest1RequestPreviewPage.CBSuperGuest;
                if (comboBox != null)
                {
                    CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
                }
            }
            else if (page is Guest1Requests guest1RequestsPage)
            {
                var comboBox = guest1RequestsPage.CBCreateReview;
                if (comboBox != null)
                {
                    CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
                }

                comboBox = guest1RequestsPage.CBSuperGuest;
                if (comboBox != null)
                {
                    CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
                }
            }
            else if (page is ShowGuest1Notifications showGuest1NotificationsPage)
            {
                var comboBox = showGuest1NotificationsPage.CBCreateReview;
                if (comboBox != null)
                {
                    CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
                }

                comboBox = showGuest1NotificationsPage.CBSuperGuest;
                if (comboBox != null)
                {
                    CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
                }
            }
            else if (page is ShowOwnerReviews showOwnerReviewsPage)
            {
                //var comboBox = showOwnerReviewsPage.CBCreateReview;
                //if (comboBox != null)
                //{
                //    CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
                //}

                //comboBox = showOwnerReviewsPage.CBSuperGuest;
                //if (comboBox != null)
                //{
                //    CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
                //}
            }
            else if (page is ShowReservations showReservationsPage)
            {
                var comboBox = showReservationsPage.CBCreateReview;
                if (comboBox != null)
                {
                    CBCreateReview.SelectedIndex = comboBox.SelectedIndex;
                }

                comboBox = showReservationsPage.CBSuperGuest;
                if (comboBox != null)
                {
                    CBSuperGuest.SelectedIndex = comboBox.SelectedIndex;
                }
            }
        }


    }
}
