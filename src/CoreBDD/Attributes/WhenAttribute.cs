namespace CoreBDD
{
    public class WhenAttribute : BDDAttribute
    {
        public WhenAttribute():base(3) { }

        public WhenAttribute(params object[] data):base(3,data) { }
    }
}