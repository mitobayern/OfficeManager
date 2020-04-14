public interface IHtmlToPdfConverter
{
    byte[] Convert(string basePath, string htmlCode);
}
