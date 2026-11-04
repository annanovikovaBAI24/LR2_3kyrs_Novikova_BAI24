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
using LR1_3kyrs_NOVIKOVA_BAI24.Model;
using LR1_3kyrs_NOVIKOVA_BAI24.ViewModel;

namespace LR1_3kyrs_NOVIKOVA_BAI24.View
{
    public partial class WindowNewEmployee : Window
    {
        public WindowNewEmployee(RoleViewModel roleVM)
        {
            InitializeComponent();

            CbRole.ItemsSource = roleVM.ListRole;// Устанавливаем источник данных для ComboBox

            Loaded += (s, e) =>// когда окно загружено
            {
                PersonDpo p = DataContext as PersonDpo;
                if (p == null) return;
                foreach (Role r in CbRole.Items)
                {
                    if (r.NameRole == p.RoleName)
                    {
                        CbRole.SelectedItem = r;
                        break;
                    }
                }
            };
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
        private void tbBirthday_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)//видимость календаря
        {
            if (tbBirthday.Visibility == Visibility.Hidden)
            {
                clBirthday.Visibility = Visibility.Visible;
            }
            else
            {
                clBirthday.Visibility = Visibility.Hidden;
            }
        }
    }
}