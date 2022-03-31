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
    /// Логика взаимодействия для DayListPage.xaml
    /// </summary>
    public partial class DayListPage : Page
    {
        private int _currPersonId;
        public DayListPage(Persons currentPerson)
        {
            _currPersonId = currentPerson.ID;
            InitializeComponent();
            //MoneyForDistance 0.5 = 0; 0.51 = 1;
        }
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                YandexFoodReportsEntities.GetContext.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                List<Days> DayList = (from d in YandexFoodReportsEntities.GetContext.Day
                                      where d.PersonID == _currPersonId
                                      join r in YandexFoodReportsEntities.GetContext.Report on d.ID equals r.DayID
                                      select new { ID = d.ID, r.AmountDelievery })
                      .GroupBy(x => x.ID, x => x.AmountDelievery)
                      .Select(g => new { ID = g.Key, TotalDelievery = g.Sum() })
                      .Join(YandexFoodReportsEntities.GetContext.Day, g => g.ID, d => d.ID, (g, d) => new Days { ID = g.ID, PersonID = d.PersonID, Date = d.Date, TotalMoney = d.TotalMoney, TotalDelievery = g.TotalDelievery })
                      .ToList();
                DGridDays.ItemsSource = DayList;
            }
        }
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {

        }
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {

            var daysToRemove = DGridDays.SelectedItems.Cast<Days>();

            if (MessageBox.Show($"Вы точно хотите удалить следующие {daysToRemove.Count()} элементов?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    var daysToRemoveID = daysToRemove.Select(x => x.ID).ToArray();
                    List<Day> days = YandexFoodReportsEntities.GetContext.Day.Where(D => daysToRemoveID.Contains(D.ID)).ToList();
                    foreach (Day day in days)
                    {
                        var reportsThisDay = (from r in YandexFoodReportsEntities.GetContext.Report
                                              where r.DayID == day.ID
                                              select r).ToList();
                        YandexFoodReportsEntities.GetContext.Report.RemoveRange(reportsThisDay);
                    }
                    YandexFoodReportsEntities.GetContext.Day.RemoveRange(days);
                    YandexFoodReportsEntities.GetContext.SaveChanges();
                    MessageBox.Show("Данные удалены!");

                    DGridDays.ItemsSource = YandexFoodReportsEntities.GetContext.Report.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            Manager.MainFrame.Navigate(new ReportListPage(DGridDays.SelectedItem as Days));
        }
    }
}