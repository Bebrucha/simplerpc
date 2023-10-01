namespace Servers
{
    using NLog;
    using Services;

    public class MeadowState
    {
        /// <summary>
        /// Access lock.
        /// </summary>
        public readonly object AccessLock = new object();

        /// <summary>
        /// Last unique ID value generated.
        /// </summary>
        public int LastUniqueId;

        /// <summary>
        /// Season state.
        /// </summary>
        public Season SeasonState;

        public int grassCount { get; set; } = 0;
        public int bugCount { get; set; } = 0;
    }

    /// <summary>
    /// Meadow logic.
    /// </summary>
    class MeadowLogic
    {
        /// <summary>
        /// Logger for this class.
        /// </summary>
        private Logger mLog = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Background task thread.
        /// </summary>
        private Thread mBgTaskThread;

        /// <summary>
        /// State descriptor.
        /// </summary>
        private MeadowState mState = new MeadowState();

        /// <summary>
        /// Constructor.
        /// </summary>
        public MeadowLogic()
        {
            // Start the background task
            mBgTaskThread = new Thread(BackgroundTask);
            mBgTaskThread.Start();
        }

          public int GetGrassCount()
        {
            lock (mState.AccessLock)
            {
                return mState.grassCount;
            }
        }

        public void AddToGrassCount(int count)
        {
            lock (mState.AccessLock)
            {
                mState.grassCount += count;
            }
        }

         public int GetBugCount()
        {
            lock (mState.AccessLock)
            {
                return mState.bugCount;
            }
        }

         public void AddToBugCount(int count)
        {
            lock (mState.AccessLock)
            {
                mState.bugCount += count;
            }
        }

        public void RemoveFromBugCount(int count)
        {
            lock (mState.AccessLock)
            {
                    mState.bugCount -= count;
            }
        }

        public void RemoveGrass(int count)
        {
            lock (mState.AccessLock)
            {
                    mState.grassCount -= count;
            }
        }
        
        public int GetUniqueId() 
	    {
		lock( mState.AccessLock )
		{
			mState.LastUniqueId += 1;
			return mState.LastUniqueId;
		}
	    }

        /// <summary>
        /// Get current season state.
        /// </summary>
        /// <returns>Current season state.</returns>				
        public Season GetSeasonState()
        {
            lock (mState.AccessLock)
            {
                return mState.SeasonState;
            }
        }

        /// <summary>
        /// Background task for the seasons.
        /// </summary>
        public void BackgroundTask()
        {
            // Initialize random number generator
            var rnd = new Random();

            while (true)
            {
                // Sleep a while
                Thread.Sleep(500 + rnd.Next(1500));

                // Switch the season
                lock (mState.AccessLock)
                {
                    mState.SeasonState =
                        mState.SeasonState == Season.Winter
                        ? Season.Spring
                        : (Season)((int)mState.SeasonState + 1);

                     mLog.Info($"New season is '{mState.SeasonState}'.");
                     mLog.Info($"Grass count is '{mState.grassCount}'.");
                     mLog.Info($"Bug count is '{mState.bugCount}'.");
                }
            }
        }
    }
}
