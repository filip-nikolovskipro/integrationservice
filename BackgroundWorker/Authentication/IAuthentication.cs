using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundWorker.Authentication
{
	public interface IAuthentication
	{
		Task<AuthToken> Authenticate();
		string Token { get; }
	}
}
