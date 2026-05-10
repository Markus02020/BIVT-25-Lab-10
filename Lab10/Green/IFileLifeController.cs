namespace Lab10.Green
{
    public interface IFileLifeController
    {
        void CreateFile();
        void DeleteFile();
        void EditFile(string text);
        void ChangeFileExtension(string ext);
    }
}
