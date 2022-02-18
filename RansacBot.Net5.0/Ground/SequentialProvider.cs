using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RansacBot
{
	class SequentialProvider<T>
	{
		Task task;
		Queue<T> allTsToProcess = new();
		Action<T> action;

		public SequentialProvider(Action<T> action)
		{
			this.action = action;
			task = Task.CompletedTask;
		}

		public void OnNewT(T t)
		{
			allTsToProcess.Enqueue(t);
			EnsureQueueIsProcessing();
		}

		void EnsureQueueIsProcessing()
		{
			if (task.IsCompleted)
			{
				task = Task.Run(() => ProcessAllTradesQueue());
			}
		}

		void ProcessAllTradesQueue()
		{
			while (allTsToProcess.Count > 0)
			{
				ProcessOneT(allTsToProcess.Dequeue());
			}
		}

		void ProcessOneT(T t)
		{
			action.Invoke(t);
		}
	}
}
