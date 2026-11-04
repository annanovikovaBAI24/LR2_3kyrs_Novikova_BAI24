using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using LR1_3kyrs_NOVIKOVA_BAI24.ViewModel;

namespace LR1_3kyrs_NOVIKOVA_BAI24.Model
{
    public class PersonDpo : INotifyPropertyChanged
    {

        public int Id { get; set; }

        private string _roleName;

        public string RoleName
        {
            get { return _roleName; }
            set
            {
                _roleName = value;
                OnPropertyChanged("RoleName");
            }
        }

        /// <summary>
        /// имя сотрудника
        /// </summary>
        private string firstName;

        /// <summary>
        /// имя сотрудника
        /// </summary>
        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                OnPropertyChanged("FirstName");
            }
        }

        /// <summary>
        /// фамилия сотрудника
        /// </summary>
        private string lastName;

        /// <summary>
        /// фамилия сотрудника
        /// </summary>
        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                OnPropertyChanged("LastName");
            }
        }

        /// <summary>
        /// дата рождения сотрудника
        /// </summary>
        private DateTime birthday;

        /// <summary>
        /// дата рождения сотрудника
        /// </summary>
        public DateTime Birthday
        {
            get { return birthday; }
            set
            {
                birthday = value;
                OnPropertyChanged("Birthday");
            }
        }

        public PersonDpo() { }

        public PersonDpo(int id, string roleName, string firstName, string lastName, DateTime birthday)
        {
            this.Id = id;
            this.RoleName = roleName;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Birthday = birthday;
        }

        /// <summary>
        /// Метод поверхностного копирования
        /// </summary>
        /// <returns></returns>
        public PersonDpo ShallowCopy()
        {
            return (PersonDpo)this.MemberwiseClone();
        }

        /// <summary>
        /// копирование данных из класса Person
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public PersonDpo CopyFromPerson(Person person)
        {
            PersonDpo perDpo = new PersonDpo();
            RoleViewModel vmRole = new RoleViewModel();
            string role = string.Empty;// Переменная для названия должности

            // ищем название должности по её коду
            foreach (var r in vmRole.ListRole)
            {
                if (r.Id == person.RoleId)
                {
                    role = r.NameRole;
                    break;
                }
            }

            // если нашли, заполняем копию
            if (role != string.Empty)
            {
                perDpo.Id = person.Id;
                perDpo.RoleName = role;
                perDpo.FirstName = person.FirstName;
                perDpo.LastName = person.LastName;
                perDpo.Birthday = person.Birthday;
            }
            return perDpo;
        }

        /// <summary>
        /// копирование данных в класс Person
        /// </summary>
        /// <param name="personDpo"></param>
        /// <returns></returns>
        public Person CopyFromPersonDPO(PersonDpo personDpo)
        {
            Person person = new Person();
            RoleViewModel vmRole = new RoleViewModel();
            int roleId = 0;

            // ищем код должности по её названию
            foreach (var r in vmRole.ListRole)
            {
                if (r.NameRole == personDpo.RoleName)
                {
                    roleId = r.Id;
                    break;
                }
            }

            person.Id = personDpo.Id;
            person.RoleId = roleId;
            person.FirstName = personDpo.FirstName;
            person.LastName = personDpo.LastName;
            person.Birthday = personDpo.Birthday;
            return person;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}