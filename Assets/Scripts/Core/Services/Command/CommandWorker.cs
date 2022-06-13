using System.Collections.Generic;
using Core.States.Battle;

namespace Core.Services.Command
{
    /// <summary>
    /// Simple command system
    /// </summary>
    public class CommandWorker : ICommandWorker
    {
        private readonly IBattleState _battleState;
        private readonly Queue<ICommandItem> _addingQueue = new Queue<ICommandItem>();
        private readonly List<ICommandItem> _commandRequests = new List<ICommandItem>();

        private bool _cancelAll;

        public CommandWorker(IBattleState battleState)
        {
            _battleState = battleState;
        }

        public void AddCommand(ICommandItem command)
        {
            _addingQueue.Enqueue(command);
        }

        public void Cancel()
        {
            _cancelAll = true;
        }

        public bool ClearProceed()
        {
            return _commandRequests.Count == 0;
        }

        public void RunUpdate(float deltaTime)
        {
            while (_addingQueue.Count > 0)
            {
                _commandRequests.Add(_addingQueue.Dequeue());
            }

            if (_cancelAll && _commandRequests.Count == 0)
            {
                _cancelAll = false;
            }

            if (_commandRequests.Count > 0)
            {
                // always pick the top of the list
                var item = _commandRequests[0];

                if (item.CmdState == CommandState.NotRunning)
                {
                    item.CmdState = item.Validate(_battleState) ? CommandState.Running : CommandState.Completed;
                }
                else if (item.CmdState == CommandState.Running)
                {
                    item.Update(deltaTime);
                }

                if (item.CmdState == CommandState.Completed || item.CmdState == CommandState.Cancelled)
                {
                    // the command is completed, remove from the list
                    _commandRequests.RemoveAt(0);
                    // add internal requests if exists
                    if (item.ChildCommands != null && item.ChildCommands.Count > 0)
                    {
                        // add internal commands at the top of the list
                        _commandRequests.InsertRange(0, item.ChildCommands);
                    }
                }
            }
        }
    }
}