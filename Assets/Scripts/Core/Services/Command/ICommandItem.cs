using System.Collections.Generic;
using Core.States.Battle;

namespace Core.Services.Command
{
    public interface ICommandItem
    {
        /// <summary>
        /// Called only once per request
        /// returns true if valid
        /// </summary>
        bool Validate(IBattleState battleState);
        /// <summary>
        /// Calls every frame until State turns to Completed
        /// </summary>
        void Update(float deltaTime);
        /// <summary>
        /// Do not forget to set as Completed when the command is done
        /// </summary>
        CommandState CmdState { get; set; }

        /// <summary>
        /// Child commands should work after this command, before the next command
        /// </summary>
        List<ICommandItem> ChildCommands { get; set; }
    }
}