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
    /// Логика взаимодействия для AddEditReportPage.xaml
    /// </summary>
    public partial class AddEditReportPage : Page
    {
        private Report _currentReport = new Report();
        public AddEditReportPage(Report currReport, int DayId)
        {
            InitializeComponent();
            if (currReport != null)
                _currentReport = currReport;
            _currentReport.DayID = DayId;
            DataContext = _currentReport;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (_currentReport.ID == 0)
                YandexFoodReportsEntities.GetContext.Report.Add(_currentReport);
            try
            {
                YandexFoodReportsEntities.GetContext.SaveChanges();
                MessageBox.Show("Сохранено");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
