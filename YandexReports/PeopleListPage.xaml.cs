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

namespace YandexReports
{
    /// <summary>
    /// Логика взаимодействия для PeopleListPage.xaml
    /// </summary>
    public partial class PeopleListPage : Page
    {
        public PeopleListPage()
        {
            InitializeComponent();
            var DayList = (from d in YandexFoodReportsEntities.GetContext.Day
                           join r in YandexFoodReportsEntities.GetContext.Report on d.ID equals r.DayID
                           select new { ID = d.ID, r.AmountDelievery })
                      .GroupBy(x => x.ID, x => x.AmountDelievery)
                      .Select(g => new { ID = g.Key, TotalDelievery = g.Sum() })
                      .Join(YandexFoodReportsEntities.GetContext.Day, g => g.ID, d => d.ID, (g, d) => new { ID = g.ID, PersonID = d.PersonID, TotalMoney = d.TotalMoney, TotalDelievery = g.TotalDelievery });

            var PersonList = (from p in YandexFoodReportsEntities.GetContext.Person
                              join d in YandexFoodReportsEntities.GetContext.Day on p.ID equals d.PersonID
                              select new { PersonID = p.ID, TotalMoney = d.TotalMoney })
                                .GroupBy(x => x.PersonID, x => x.TotalMoney)
                                .Select(g => new { ID = g.Key, TotalMoney = g.Sum() })
                                .Join(YandexFoodReportsEntities.GetContext.Person, g => g.ID, p => p.ID, (g, p) => new { ID = g.ID, Name = p.Name, TotalMoney = g.TotalMoney });

            var dataContext = (from p in PersonList
                               join d in DayList on p.ID equals d.PersonID
                               select new { PersonID = p.ID, TotalDelievery = d.TotalDelievery })
                               .GroupBy(x => x.PersonID, x => x.TotalDelievery)
                               .Select(g => new { ID = g.Key, TotalDelievery = g.Sum() })
                               .Join(PersonList, g => g.ID, p => p.ID, (g, p) => new Persons { ID = g.ID, Name = p.Name, TotalMoney = p.TotalMoney, TotalDelievery = g.TotalDelievery })
                               .ToList();
            DGridPeople.ItemsSource = dataContext;
        }
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                YandexFoodReportsEntities.GetContext.ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                var DayList = (from d in YandexFoodReportsEntities.GetContext.Day
                               join r in YandexFoodReportsEntities.GetContext.Report on d.ID equals r.DayID
                               select new { ID = d.ID, r.AmountDelievery })
                      .GroupBy(x => x.ID, x => x.AmountDelievery)
                      .Select(g => new { ID = g.Key, TotalDelievery = g.Sum() })
                      .Join(YandexFoodReportsEntities.GetContext.Day, g => g.ID, d => d.ID, (g, d) => new { ID = g.ID, PersonID = d.PersonID, TotalMoney = d.TotalMoney, TotalDelievery = g.TotalDelievery })
                      .DefaultIfEmpty();

                var PersonList = (from p in YandexFoodReportsEntities.GetContext.Person
                                  join d in YandexFoodReportsEntities.GetContext.Day on p.ID equals d.PersonID
                                  select new { PersonID = p.ID, TotalMoney = d.TotalMoney })
                                    .GroupBy(x => x.PersonID, x => x.TotalMoney)
                                    .Select(g => new { ID = g.Key, TotalMoney = g.Sum() })
                                    .Join(YandexFoodReportsEntities.GetContext.Person, g => g.ID, p => p.ID, (g, p) => new { ID = g.ID, Name = p.Name, TotalMoney = g.TotalMoney })
                                    .DefaultIfEmpty();

                var dataContext = (from p in PersonList
                                   join d in DayList on p.ID equals d.PersonID
                                   select new { PersonID = p.ID, TotalDelievery = d.TotalDelievery })
                                   .GroupBy(x => x.PersonID, x => x.TotalDelievery)
                                   .Select(g => new { ID = g.Key, TotalDelievery = g.Sum() })
                                   .Join(PersonList, g => g.ID, p => p.ID, (g, p) => new Persons { ID = g.ID, Name = p.Name, TotalMoney = p.TotalMoney, TotalDelievery = g.TotalDelievery })
                                   .DefaultIfEmpty()
                                   .ToList();
                DGridPeople.ItemsSource = dataContext;
            }
        }
        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            Manager.MainFrame.Navigate(new DayListPage(DGridPeople.SelectedItem as Persons));
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var peopleForRemoving = DGridPeople.SelectedItems.Cast<Person>().ToList();

            if (MessageBox.Show($"Вы точно хотите удалить следующие {peopleForRemoving.Count()} элементов?", "", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    YandexFoodReportsEntities.GetContext.Person.RemoveRange(peopleForRemoving);
                    YandexFoodReportsEntities.GetContext.SaveChanges();
                    MessageBox.Show("Данные удалены!");

                    DGridPeople.ItemsSource = YandexFoodReportsEntities.GetContext.Report.ToList();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
    }
}
