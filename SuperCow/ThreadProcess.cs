using System.Threading;

namespace SuperCow;

public class ThreadProcess
{
	private WaitCallback callback;

	public ThreadProcess(WaitCallback target)
	{
		callback = target;
	}

	public void Start()
	{
		ThreadPool.QueueUserWorkItem(callback);
	}
}
