using IdentifyMe.Messages;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace IdentifyMe.Services
{
    public class TaskCounter
    {
		public async Task RunCounter(CancellationToken token)
		{
			await Task.Run(async () => {

				//for (long i = 0; i < long.MaxValue; i++)
				long i = 0;
				while (true)
				{
					i++;
					token.ThrowIfCancellationRequested();
					var message = new TickedMessage
					{
						Message = i.ToString()
					};
					Device.BeginInvokeOnMainThread(() => {
						MessagingCenter.Send<TickedMessage>(message, "TickedMessage");
					});
					await Task.Delay(15000);

				}
			}, token);
		}
	}
}
