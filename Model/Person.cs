using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LR1_3kyrs_NOVIKOVA_BAI24.ViewModel;

namespace LR1_3kyrs_NOVIKOVA_BAI24.Model
{
    public class Person
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Birthday { get; set; } // Изменено с DateTime на string

        public Person() { }// Пустой конструктор для десериализации JSON

        public Person(int id, int roleId, string firstName, string lastName, string birthday)
        {
            this.Id = id;
            this.RoleId = roleId;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Birthday = birthday;
        }

        /// <summary>
        /// копирование данных из класса PersonDpo
        /// </summary>
        /// <param name="personDpo"></param>
        /// <returns></returns>
        public Person CopyFromPersonDPO(PersonDpo personDpo)
        {
            Person person = new Person();
            RoleViewModel vmRole = new RoleViewModel();// Создаём RoleViewModel, чтобы получить ListRole
            int roleId = 0;
            foreach (var r in vmRole.ListRole)
            {
                if (r.NameRole == personDpo.RoleName)
                {
                    roleId = r.Id;
                    break;
                }
            }
            person.Id = personDpo.Id;// Заполняем новый Person
            person.RoleId = roleId;
            person.FirstName = personDpo.FirstName;
            person.LastName = personDpo.LastName;
            person.Birthday = personDpo.Birthday; // Теперь присваиваем строку
            return person;
        }
    }
}