using System;

namespace CoreBDD
{
    public class Example : System.Attribute
    {
        public string Title { get; set; }
        public Example(string title)
        {
            Title = title;
        }

    }
}