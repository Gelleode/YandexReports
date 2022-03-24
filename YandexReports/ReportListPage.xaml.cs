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

namespace YandexReports
{
    /// <summary>
    /// Логика взаимодействия для ReportListPage.xaml
    /// </summary>
    public partial class ReportListPage : Page
    {
        private int _currDayID;
        public ReportListPage(Days currentDay)
        {
            _currDayID = currentDay.ID;
            InitializeComponent();
            List<Report> dataContext = (from r in YandexFoodReportsEntities.GetContext.Report
                                        where r.DayID == _currDayID
                                        select r)
                              .ToList();
            DGridReports.ItemsSource = dataContext;
        }
        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var reportsForRemoving = DGridReports.SelectedItems.Cast<Report>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующие {reportsForRemoving.Count()} элементов?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    YandexFoodReportsEntities.GetContext.Report.RemoveRange(reportsForRemoving);
                    YandexFoodReportsEntities.GetContext.SaveChanges();
                    MessageBox.Show("Данные удалены!");

                    DGridReports.ItemsSource = YandexFoodReportsEntities.GetContext.Report.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                YandexFoodReportsEntities.GetContext.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                DGridReports.ItemsSource = (from r in YandexFoodReportsEntities.GetContext.Report
                                            where r.DayID == _currDayID
                                            select r)
                              .ToList();
            }
        }
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
