namespace Core.Services.Command
{
    public interface ICommandWorker
    {
        void AddCommand(ICommandItem command);
        void Cancel();
        void RunUpdate(float deltaTime);
        /// <summary>
        /// To check is there any command processing
        /// </summary>
        /// <returns></returns>
        bool ClearProceed();
    }
}