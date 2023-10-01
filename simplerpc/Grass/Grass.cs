namespace Clients;

using Microsoft.Extensions.DependencyInjection;

using SimpleRpc.Serialization.Hyperion;
using SimpleRpc.Transports;
using SimpleRpc.Transports.Http.Client;

using NLog;

using Services;



class GrassClient
{
	Logger mLog = LogManager.GetCurrentClassLogger();

	private void ConfigureLogging()
	{
		var config = new NLog.Config.LoggingConfiguration();

		var console =
			new NLog.Targets.ConsoleTarget("console")
			{
				Layout = @"${date:format=HH\:mm\:ss}|${level}| ${message} ${exception}"
			};
		config.AddTarget(console);
		config.AddRuleForAllLevels(console);

		LogManager.Configuration = config;
	}


	private void Run() {
	
		ConfigureLogging();

		
		var rnd = new Random();

	
		while( true )
		{
			try {
				
				var sc = new ServiceCollection();
				sc
					.AddSimpleRpcClient(
						"meadowService", 
						new HttpClientTransportOptions
						{
							Url = "http://127.0.0.1:5000/simplerpc",
							Serializer = "HyperionMessageSerializer"
						}
					)
					.AddSimpleRpcHyperionSerializer();

				sc.AddSimpleRpcProxy<IMeadowService>("meadowService"); 

				var sp = sc.BuildServiceProvider();

				var meadow = sp.GetService<IMeadowService>();


                var grass = new GrassDesc();
               
			
				grass.grassId = meadow.GetUniqueId();
		
				while( true )
				{
					grass= new GrassDesc {
 					grassGrowth = rnd.Next(20,100), 
               		 };
                        var season = meadow.getCurrentSeason();

						mLog.Info($"grass client id: {grass.grassId}, It is growing: {grass.grassGrowth}");
						Console.Title = $"grass client id: {grass.grassId}, It is growing: {grass.grassGrowth}";
						
						   Thread.Sleep(500 + rnd.Next(1500));
                        if (season == Season.Spring || season == Season.Summer)
                        {		
								Console.WriteLine("Adding grass to the meadow currently in season: " + season);
								meadow.AddToGrassCount(grass.grassGrowth);
                        }

				}				
			}
			catch( Exception e )
			{
				
				mLog.Warn(e, "Unhandled exception caught. Will restart main loop.");

			
				Thread.Sleep(2000);
			}
		}
	}


	
	static void Main(string[] args)
	{
		var self = new GrassClient();
		self.Run();
	}
}
