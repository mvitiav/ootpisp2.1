namespace ConsoleApp1
{
    public class fileWriter : IoutWriter
    {
        private string path;

        public fileWriter(string path)
        {
            this.path = path;
        }

        public void save(string result)
        {
            System.IO.File.WriteAllText(path, result);
        }
    }
}