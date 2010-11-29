namespace ASP.App_Code
{
    public class SampleClass : ISampleClass
    {
        public bool DoesThisContainThat(string thisString, string thatString)
        {
            return thisString.Contains(thatString);
        }

        public object GetNull()
        {
            return null;
        }
    }
}