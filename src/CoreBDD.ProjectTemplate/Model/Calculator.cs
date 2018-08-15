namespace CoreBDD.ProjectTemplate
{
     public class Calculator
    {
        public string Equation { get; set; }
        public int Total   { get; set; }
        public int Add(int x, int y) => x + y;
        public int Subtract(int x, int y) => x - y;
        public Calculator Key(int value)      { Total = value;  Equation = value.ToString(); return this; }
        public Calculator Add(int value)      { Total += value; Equation += " + " + value;   return this; }
        public Calculator Subtract(int value) { Total -= value; Equation += " - " + value;   return this; }
        public Calculator Multiply(int value) { Total *= value; Equation += " x " + value;   return this; }
        public Calculator Divide(int value)   { Total /= value; Equation += " / " + value;   return this; }
        public Calculator Clear()                 { Total = 0;      Equation = "";               return this; }
    }
}