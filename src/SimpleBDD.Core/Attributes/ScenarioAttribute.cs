using System;

namespace SimpleBDD
{
    public class ScenarioAttribute : System.Attribute
    {
        public string Title { get; set; }
        public ScenarioAttribute(string title)
        {
            Title = title;
        }

    }
}