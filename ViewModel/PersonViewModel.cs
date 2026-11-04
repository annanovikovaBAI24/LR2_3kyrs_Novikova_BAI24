using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using Newtonsoft.Json; 
using LR1_3kyrs_NOVIKOVA_BAI24.Helper;
using LR1_3kyrs_NOVIKOVA_BAI24.Model;
using LR1_3kyrs_NOVIKOVA_BAI24.View;

namespace LR1_3kyrs_NOVIKOVA_BAI24.ViewModel
{
    public class PersonViewModel : INotifyPropertyChanged
    {
        public RoleViewModel RoleVM { get; set; }
        readonly string path = @"C:\Users\Anna\OneDrive\Desktop\вп 3 курс\LR1_3kyrs_Novikova_BAI24\DataModels\PersonData.json";

        public ObservableCollection<Person> ListPerson { get; set; } = new ObservableCollection<Person>();
        public ObservableCollection<PersonDpo> ListPersonDpo { get; set; } = new ObservableCollection<PersonDpo>();

        public string Error { get; set; }
        string _jsonPersons = String.Empty;

        public PersonViewModel(RoleViewModel roleVM)
        {
            RoleVM = roleVM;

            ListPerson = LoadPerson(); // Загружаем данные из JSON
            ListPersonDpo = GetListPersonDpo(); // Преобразуем в PersonDpo для отображения
        }

        // Метод загрузки из JSON
        public ObservableCollection<Person> LoadPerson()
        {
            if (!File.Exists(path))
            {
                return new ObservableCollection<Person>();
            }
            _jsonPersons = File.ReadAllText(path);
            if (_jsonPersons != null)
            {
                ListPerson = JsonConvert.DeserializeObject<ObservableCollection<Person>>(_jsonPersons);
                return ListPerson;
            }
            return null;
        }

        // Метод сохранения в JSON
        private void SaveChanges(ObservableCollection<Person> listPersons)
        {
            var jsonPerson = JsonConvert.SerializeObject(listPersons);
            try
            {
                using (StreamWriter writer = File.CreateText(path))
                {
                    writer.Write(jsonPerson);
                }
            }
            catch (IOException e)
            {
                Error = "Ошибка записи json файла \n" + e.Message;
            }
        }

        private PersonDpo selectedPersonDpo;
        public PersonDpo SelectedPersonDpo
        {
            get { return selectedPersonDpo; }
            set
            {
                selectedPersonDpo = value;
                OnPropertyChanged("SelectedPersonDpo");
            }
        }

        public ObservableCollection<PersonDpo> GetListPersonDpo()
        {
            var listDpo = new ObservableCollection<PersonDpo>();
            foreach (var person in ListPerson)
            {
                PersonDpo p = new PersonDpo();
                p = p.CopyFromPerson(person);
                listDpo.Add(p);
            }
            return listDpo;
        }

        public int MaxId()
        {
            int max = 0;
            foreach (var r in this.ListPerson)
            {
                if (max < r.Id) { max = r.Id; }
            }
            return max;
        }

        #region AddPerson
        private RelayCommand addPerson;
        public RelayCommand AddPerson
        {
            get
            {
                return addPerson ??
                (addPerson = new RelayCommand(obj =>
                {
                    WindowNewEmployee wnPerson = new WindowNewEmployee(RoleVM) { Title = "Новый сотрудник" };
                    int maxIdPerson = MaxId() + 1;

                    PersonDpo per = new PersonDpo
                    {
                        Id = maxIdPerson,
                        Birthday = DateTime.Now.ToString("dd.MM.yyyy") // Устанавливаем строку
                    };

                    wnPerson.DataContext = per;
                    if (wnPerson.ShowDialog() == true)
                    {
                        Role r = (Role)wnPerson.CbRole.SelectedValue;
                        if (r != null)
                        {
                            per.RoleName = r.NameRole;
                            ListPersonDpo.Add(per);

                            Person p = new Person();
                            p = p.CopyFromPersonDPO(per);
                            ListPerson.Add(p);
                            SaveChanges(ListPerson); // Сохраняем в JSON
                        }
                    }
                },
                (obj) => true));
            }
        }
        #endregion

        #region EditPerson
        private RelayCommand editPerson;
        public RelayCommand EditPerson
        {
            get
            {
                return editPerson ??
                (editPerson = new RelayCommand(obj =>
                {
                    WindowNewEmployee wnPerson = new WindowNewEmployee(RoleVM) { Title = "Редактирование данных сотрудника" };
                    PersonDpo personDpo = SelectedPersonDpo;
                    PersonDpo tempPerson = new PersonDpo();
                    tempPerson = personDpo.ShallowCopy();
                    wnPerson.DataContext = tempPerson;
                    if (wnPerson.ShowDialog() == true)
                    {
                        Role r = (Role)wnPerson.CbRole.SelectedValue;
                        if (r != null)
                        {
                            personDpo.RoleName = r.NameRole;
                            personDpo.FirstName = tempPerson.FirstName;
                            personDpo.LastName = tempPerson.LastName;
                            personDpo.Birthday = tempPerson.Birthday;

                            FindPerson finder = new FindPerson(personDpo.Id);
                            List<Person> listPerson = ListPerson.ToList();
                            Person p = listPerson.Find(new Predicate<Person>(finder.PersonPredicate));
                            p = p.CopyFromPersonDPO(personDpo);
                            SaveChanges(ListPerson); // Сохраняем в JSON
                        }
                    }
                },
                (obj) => SelectedPersonDpo != null && ListPersonDpo.Count > 0));
            }
        }
        #endregion

        #region DeletePerson
        private RelayCommand deletePerson;
        public RelayCommand DeletePerson
        {
            get
            {
                return deletePerson ??
                (deletePerson = new RelayCommand(obj =>
                {
                    PersonDpo person = SelectedPersonDpo;
                    MessageBoxResult result = MessageBox.Show("Удалить данные по сотруднику: \n" + person.LastName + " " + person.FirstName,
                        "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.OK)
                    {
                        ListPersonDpo.Remove(person);
                        Person per = new Person();
                        per = per.CopyFromPersonDPO(person);
                        ListPerson.Remove(per);
                        SaveChanges(ListPerson); // Сохраняем в JSON
                    }
                },
                (obj) => SelectedPersonDpo != null && ListPersonDpo.Count > 0));
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}