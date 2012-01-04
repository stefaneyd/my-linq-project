using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Extentions;
using System.Linq.Expressions;

namespace LINQ_Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            DateTime date = new DateTime(2002, 8, 9);
            int daysTillEndOfMonth = date.DaysToEndOfMonth();  //the function DaysToEndOfMonth shows up like an instance function
            IEnumerable<string> cities = new[] { "Ghent", "London", "Las Vegas", "Hyderabad" };

            IEnumerable<string> query = cities.StringThatStartWith("L");
            foreach (string city in query)
                Console.WriteLine(city);

            Console.WriteLine();
            IEnumerable<string> query2 = cities.Filter(StringThatStartWithL);
            foreach (string city in query2)
                Console.WriteLine(city);

            Console.WriteLine();
            IEnumerable<string> query3 = cities.Filter(delegate(string item) { return item.StartsWith("L"); });
            foreach (string city in query3)
                Console.WriteLine(city);

            Console.WriteLine();
            IEnumerable<string> query4 = cities.Filter((item) => item.StartsWith("L"));
            foreach (string city in query4)
                Console.WriteLine(city);
            
            Console.WriteLine();
            IEnumerable<string> query5 = cities.Where(city => city.StartsWith("L")).OrderByDescending(city => city.Length);
            foreach (string city in query5)
                Console.WriteLine(city);

            Console.WriteLine();
            var query6 = from c in cities where c.StartsWith("L") && c.Length < 8 orderby c select c;
            var query7 = cities.Where(c => c.StartsWith("L") && c.Length < 8).OrderBy(c => c).Select(c => c); //alveg sama query og qurey6
            foreach (string city in query7)
                Console.WriteLine(city);

            Console.WriteLine();
            WorkingWithFunc();
        }

        private static void WorkingWithFunc()
        {
            Func<int, int> square = x => x * x;
            Func<int, int, int> add = (x, y) => x + y;
            Expression<Func<int, int>> square2 = x => x * x;
            Action<int> write = x => Console.WriteLine(x);

            write(square(add(1, 1)));
        }

        static bool StringThatStartWithL(String str)
        {
            return str.StartsWith("L");
        }
    }
}

namespace Extentions
{
    /// <summary>
    /// Adding "this" keyword to a static method in a static class
    /// makes it an extention method. That will make it look like an instance method
    /// from that class that has the keyword "this" in front of it.
    /// </summary>
    public static class DateUtilities
    {
        public static int DaysToEndOfMonth(this DateTime date)
        {
            return DateTime.DaysInMonth(date.Year, date.Month) - date.Day;
        }
    }

    public static class FilterExtensions
    {
        /// <summary>
        /// yield returns an implemented IEnumerable 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public static IEnumerable<string> StringThatStartWith
            (this IEnumerable<string> input, string start)
        {
            foreach (string s in input)
            {
                if (s.StartsWith(start))
                    yield return s;
            }
        }

        public static IEnumerable<T> Filter<T>(this IEnumerable<T> input, FilterDelegate<T> predicate)
        {
            foreach (var item in input)
            {
                if (predicate(item))
                    yield return item;
            }
        }

        public static IEnumerable<T> Filter2<T>(this IEnumerable<T> input, Func<T, bool> predicate)
        {
            foreach (var item in input)
            {
                if (predicate(item))
                    yield return item;
            }
        }

        public delegate bool FilterDelegate<T>(T item);
    }
}
