namespace CLI_Engine
{
    public interface IParamsFactory
    {
        object[] Create(string[] args);
    }
}