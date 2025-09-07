namespace StringContains
{
    public interface ITestCase
    {
        public void Load (string[] searchFor);

        public int FindAll (string str);
    }
}