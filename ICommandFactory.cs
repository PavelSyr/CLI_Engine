namespace CLI_Engine
{
    public interface ICommandFactory
    {
        ICommand Create(string[] args);
    }
}