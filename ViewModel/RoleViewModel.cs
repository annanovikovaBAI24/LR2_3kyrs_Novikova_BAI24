using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using Newtonsoft.Json; // Подключаем библиотеку для JSON
using LR1_3kyrs_NOVIKOVA_BAI24.Helper;
using LR1_3kyrs_NOVIKOVA_BAI24.Model;
using LR1_3kyrs_NOVIKOVA_BAI24.View;

namespace LR1_3kyrs_NOVIKOVA_BAI24.ViewModel
{
    public class RoleViewModel : INotifyPropertyChanged
    {
    
        readonly string path = @"C:\Users\Anna\OneDrive\Desktop\вп 3 курс\LR1_3kyrs_Novikova_BAI24\DataModels\RoleData.json";

        public ObservableCollection<Role> ListRole { get; set; } = new ObservableCollection<Role>();

        private Role selectedRole;
        public Role SelectedRole
        {
            get { return selectedRole; }
            set
            {
                selectedRole = value;
                OnPropertyChanged("SelectedRole");
                EditRole.CanExecute(true);
            }
        }

        public string Error { get; set; }
        string _jsonRoles = String.Empty;

        public RoleViewModel()
        {
            ListRole = LoadRole(); // Загружаем данные при старте
        }

        // Метод загрузки из JSON
        public ObservableCollection<Role> LoadRole()
        {
            if (!File.Exists(path))
            {
                return new ObservableCollection<Role>(); // Если файла нет, возвращаем пустоту
            }
            _jsonRoles = File.ReadAllText(path);
            if (_jsonRoles != null)
            {
                ListRole = JsonConvert.DeserializeObject<ObservableCollection<Role>>(_jsonRoles);
                return ListRole;
            }
            return null;
        }

        // Метод сохранения в JSON
        private void SaveChanges(ObservableCollection<Role> listRole)
        {
            var jsonRole = JsonConvert.SerializeObject(listRole);
            try
            {
                using (StreamWriter writer = File.CreateText(path))
                {
                    writer.Write(jsonRole);
                }
            }
            catch (IOException e)
            {
                Error = "Ошибка записи json файла \n" + e.Message;
            }
        }

        public int MaxId()
        {
            int max = 0;
            foreach (var r in this.ListRole)
            {
                if (max < r.Id) { max = r.Id; }
            }
            return max;
        }

        private RelayCommand addRole;
        public RelayCommand AddRole
        {
            get
            {
                return addRole ??
                (addRole = new RelayCommand(obj =>
                {
                    WindowNewRole wnRole = new WindowNewRole { Title = "Новая должность" };
                    int maxIdRole = MaxId() + 1;
                    Role role = new Role { Id = maxIdRole };
                    wnRole.DataContext = role;
                    if (wnRole.ShowDialog() == true)
                    {
                        ListRole.Add(role);
                        SaveChanges(ListRole); // Сохраняем в JSON после добавления
                    }
                    SelectedRole = role;
                }));
            }
        }

        private RelayCommand editRole;
        public RelayCommand EditRole
        {
            get
            {
                return editRole ??
                (editRole = new RelayCommand(obj =>
                {
                    WindowNewRole wnRole = new WindowNewRole { Title = "Редактирование должности" };
                    Role role = SelectedRole;
                    Role tempRole = new Role();
                    tempRole = role.ShallowCopy();
                    wnRole.DataContext = tempRole;
                    if (wnRole.ShowDialog() == true)
                    {
                        role.NameRole = tempRole.NameRole;
                        SaveChanges(ListRole); // Сохраняем в JSON после редактирования
                    }
                },
                (obj) => SelectedRole != null && ListRole.Count > 0));
            }
        }

        private RelayCommand deleteRole;
        public RelayCommand DeleteRole
        {
            get
            {
                return deleteRole ??
                (deleteRole = new RelayCommand(obj =>
                {
                    Role role = SelectedRole;
                    MessageBoxResult result = MessageBox.Show("Удалить данные по должности: " +
                        role.NameRole, "Предупреждение", MessageBoxButton.OKCancel,
                        MessageBoxImage.Warning);
                    if (result == MessageBoxResult.OK)
                    {
                        ListRole.Remove(role);
                        SaveChanges(ListRole); // Сохраняем в JSON после удаления
                    }
                },
                (obj) => SelectedRole != null && ListRole.Count > 0));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}