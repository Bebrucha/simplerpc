namespace Clients;

using Microsoft.Extensions.DependencyInjection;

using SimpleRpc.Serialization.Hyperion;
using SimpleRpc.Transports;
using SimpleRpc.Transports.Http.Client;

using NLog;

using Services;



class BugClient
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


                var bug = new BugDesc();

				bug.bugId = meadow.GetUniqueId();
		
					while( true )
				{
					bug= new BugDesc{       
                    bugGrowth = rnd.Next(-1, 6), 
                    bugDecline = rnd.Next(1, 5),  
                    bugEats = rnd.Next(1, 10)   
                	};
                    var season = meadow.getCurrentSeason();

					mLog.Info($"bug client id: {bug.bugId}, Growth: {bug.bugGrowth}, Decline: {bug.bugDecline}, Consuming: {bug.bugEats}.");
					Console.Title = $"bug client id: {bug.bugId}, Growth: {bug.bugGrowth}, Decline: {bug.bugDecline}, Consuming: {bug.bugEats}.";

						   Thread.Sleep(500 + rnd.Next(1500));
                        if (season == Season.Spring || season == Season.Summer)
                        {		
							Console.WriteLine("Adding bugs to the meadow currently in season: " + season);
							meadow.AddToBugCount(bug.bugGrowth);
							
							int grassConsumed = bug.bugEats * meadow.GetBugCount();
							if(meadow.GetGrassCount()-grassConsumed >= 0)
							{
							Console.WriteLine("Removing grass from the meadow currently in season: " + season + " Grass consumed: " + grassConsumed);
           					meadow.RemoveGrass(grassConsumed);
							}
							else
							{
							Console.WriteLine("Not enough grass to satisfy every bug. Grass count: " + meadow.GetGrassCount() + " Grass consumed: " + meadow.GetGrassCount());
							meadow.RemoveGrass(meadow.GetGrassCount());
							}
						}
						else if (season == Season.Fall)
						{
							Console.WriteLine("Removing bugs from the meadow currently in season: " + season);
							meadow.RemoveFromBugCount(bug.bugDecline);
                        }

				}				
			}
			catch( Exception e )
			{
				mLog.Warn(e, "Unhandled exception caught. Will restart main loop.");

				//prevent console spamming
				Thread.Sleep(2000);
			}
		}
	}


	static void Main(string[] args)
	{
		var self = new BugClient();
		self.Run();
	}
}
