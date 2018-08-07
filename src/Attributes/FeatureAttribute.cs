using System;

namespace SimpleBDD
{
    public class FeatureAttribute : Attribute
    {
         public FeatureAttribute(string title)
        {
            this.Title = title;

        }
        public FeatureAttribute(string title, string description)
        {
            this.Title = title;
            this.Description = description;

        }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}