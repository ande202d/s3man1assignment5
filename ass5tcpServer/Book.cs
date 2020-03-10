using System;
using System.Collections.Generic;
using System.Text;

namespace ass5tcpServer
{
    public class Book
    {
        private string _title;
        private string _forfatter;
        private int _sidetal;
        private string _isbn13;

        public string ISBN13
        {
            get { return _isbn13; }
            set
            {
                if (value.Length == 13) _isbn13 = value;
                else throw new Exception("ISBN13 skal være præcis 13 tegn lang");

            }
        }
        public int Sidetal
        {
            get { return _sidetal; }
            set
            {
                if (value >= 4 && value <= 1000) _sidetal = value;
                else throw new Exception("Sidetal skal være imellem 4 og 1000");
            }
        }
        public string Forfatter
        {
            get { return _forfatter; }
            set
            {
                if (value.Length >= 2) _forfatter = value;
                else throw new Exception("Forfatter skal være minimum 2 tegn lang");

            }
        }
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public Book()
        {

        }

        public Book(string title, string forfatter, int sidetal, string isbn13)
        {
            try { _title = title; }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            try { _forfatter = forfatter; }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            try { _sidetal = sidetal; }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            try { _isbn13 = isbn13; }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
