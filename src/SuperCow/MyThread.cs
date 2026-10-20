using System.Threading;

namespace SuperCow;

internal class MyThread
{
	private Thread mHandle;

	private ThreadInterface mListener;

	public bool Start(ThreadInterface listener, ThreadPriority priority)
	{
		if (mHandle != null)
		{
			return false;
		}
		mHandle = new Thread(ThreadProc);
		mHandle.Priority = priority;
		if (mHandle != null)
		{
			mListener = listener;
			mHandle.Start();
			return true;
		}
		return false;
	}

	protected void Run()
	{
		mListener.ThreadProc();
	}

	protected void ThreadProc()
	{
		Run();
		mListener.ThreadFinished();
	}
}
