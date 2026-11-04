using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LR1_3kyrs_NOVIKOVA_BAI24.Model;

namespace LR1_3kyrs_NOVIKOVA_BAI24.Helper
{
    public class FindPerson
    {
        int id;

        public FindPerson(int id)
        {
            this.id = id;
        }

        public bool PersonPredicate(Person person)// Метод PersonPredicate проверяет, совпадает ли Id сотрудника с искомым
        {
            return person.Id == id;
        }
    }
}