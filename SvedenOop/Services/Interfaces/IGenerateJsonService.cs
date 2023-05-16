namespace SvedenOop.Services.Interfaces
{
    public interface IGenerateJsonService
    {
        public Task GenerateJsonFileForOopDgu();
        public string GetGeneratedJsonFileForOopDgu();
        public void GenerateJsonFileForOpop2();
        public string GetGeneratedJsonFileForOpop2();
    }
}
