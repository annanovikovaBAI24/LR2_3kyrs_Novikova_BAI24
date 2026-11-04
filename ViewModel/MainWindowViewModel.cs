using System.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LR1_3kyrs_NOVIKOVA_BAI24.Helper;
using LR1_3kyrs_NOVIKOVA_BAI24.View;

namespace LR1_3kyrs_NOVIKOVA_BAI24.ViewModel
{
    public class MainWindowViewModel
    {
        public RoleViewModel RoleVM { get; set; }
        public PersonViewModel PersonVM { get; set; }

        public MainWindowViewModel()
        {
            RoleVM = new RoleViewModel();
            PersonVM = new PersonViewModel(RoleVM); // передаём RoleVM
        }

        private RelayCommand openEmployee;
        public RelayCommand OpenEmployee
        {
            get
            {
                return openEmployee ??
                (openEmployee = new RelayCommand(obj =>
                {
                    WindowEmployee wnEmployee = new WindowEmployee
                    {
                        Title = "Сотрудники"
                    };
                    wnEmployee.DataContext = PersonVM;
                    wnEmployee.ShowDialog();
                }));
            }
        }

        private RelayCommand openRole;
        public RelayCommand OpenRole
        {
            get
            {
                return openRole ??
                (openRole = new RelayCommand(obj =>
                {
                    WindowRole wnRole = new WindowRole
                    {
                        Title = "Должности"
                    };
                    wnRole.DataContext = RoleVM;
                    wnRole.ShowDialog();
                }));
            }
        }

        private RelayCommand exitApp;
        public RelayCommand ExitApp
        {
            get
            {
                return exitApp ??
                (exitApp = new RelayCommand(obj =>
                {
                    Application.Current.Shutdown();
                }));
            }
        }
    }
}