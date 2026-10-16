using System.Collections.Generic;
using System.Threading;

namespace SuperCow;

public class ManagedThread
{
	private Thread thread;

	private int processorAffinity;

	private bool killThread;

	private Queue<ThreadTask> tasks = new Queue<ThreadTask>();

	public ManagedThread(int processorAffinity, ThreadPriority priority)
	{
		this.processorAffinity = processorAffinity;
		ThreadStart start = taskRunner;
		thread = new Thread(start);
		thread.Priority = priority;
		thread.Start();
	}

	public ManagedThread()
		: this(-1, ThreadPriority.Normal)
	{
	}

	private void taskRunner()
	{
		if (processorAffinity > 0 && processorAffinity < 6 && processorAffinity != 2)
		{
			thread.SetProcessorAffinity(new int[1] { processorAffinity });
		}
		while (!killThread)
		{
			if (tasks.Count > 0)
			{
				ThreadTask threadTask;
				lock (tasks)
				{
					threadTask = tasks.Dequeue();
				}
				threadTask.Task();
				if (threadTask.TaskFinished != null)
				{
					threadTask.TaskFinished(threadTask.TaskId);
				}
			}
			else
			{
				Thread.Sleep(0);
			}
		}
		tasks.Clear();
		tasks = null;
	}

	public void Kill()
	{
		killThread = true;
	}

	public void KillImmediately()
	{
		killThread = true;
		thread.Abort();
	}

	public void AddTask(ThreadTask task)
	{
		lock (tasks)
		{
			tasks.Enqueue(task);
		}
	}
}
