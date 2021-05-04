using System;
using System.IO;
using System.Linq;
using System.Net;

namespace TestApp1 {
	public static class Program {
		public static void Main(string[] args) {
			UInt16 port = 8080;

			if (args.FirstOrDefault() is var arg && UInt16.TryParse(arg, out var argPort)) {
				port = argPort;
			}

			var listener = new HttpListener() {
				Prefixes = { $"http://+:{port}/" }
			};

			listener.Start();

			Console.WriteLine("{0:u} listening at Port {1}", DateTimeOffset.UtcNow, port);

			while (listener.GetContext() is var ctx) {
				var response = $"{DateTimeOffset.UtcNow:u}\t{ctx.Request.RemoteEndPoint}\t{ctx.Request.LocalEndPoint}";
				Console.WriteLine(response);

				using var resp = ctx.Response;
				using var output = resp.OutputStream;
				using var writer = new StreamWriter(output);

				writer.Write(response);
			}
		}
	}
}