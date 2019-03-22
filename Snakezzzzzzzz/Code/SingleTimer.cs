using System.Threading;

namespace Snakezzzzzzzz.Code
{
	public class SingleTimer
	{
		private Timer timer;

		public void Run(TimerCallback timerCallBack)
		{
			if (timer == null)
			{
				this.timer = new Timer(timerCallBack, null, 0, 1000 / 15);
			}
		}
	}
}
