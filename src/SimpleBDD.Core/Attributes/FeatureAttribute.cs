using System;

namespace SimpleBDD
{
    public class Feature : System.Attribute
    {
         public Feature(string title)
        {
            this.Title = title;

        }
        public Feature(string title, string description)
        {
            this.Title = title;
            this.Description = description;

        }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}