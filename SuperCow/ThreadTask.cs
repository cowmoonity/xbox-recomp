namespace SuperCow;

public struct ThreadTask
{
	private ThreadTaskDelegate task;

	private TaskFinishedDelegate taskFinished;

	private int taskId;

	public ThreadTaskDelegate Task => task;

	public TaskFinishedDelegate TaskFinished => taskFinished;

	public int TaskId => taskId;

	public ThreadTask(ThreadTaskDelegate task, TaskFinishedDelegate taskFinished, int taskId)
	{
		this.task = task;
		this.taskFinished = taskFinished;
		this.taskId = taskId;
	}

	public ThreadTask(ThreadTaskDelegate task, TaskFinishedDelegate taskFinished)
		: this(task, taskFinished, -1)
	{
	}

	public ThreadTask(ThreadTaskDelegate task)
		: this(task, null, -1)
	{
	}
}
