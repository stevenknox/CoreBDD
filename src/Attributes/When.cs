namespace SimpleBDD
{
    public class When : BDDAttribute
    {
        public When():base(3) { }

        public When(params object[] data):base(3,data) { }
    }
}