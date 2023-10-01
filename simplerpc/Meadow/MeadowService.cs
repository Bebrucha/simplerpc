namespace Servers;

using Services;

/// <summary>
/// Service
/// </summary>
public class MeadowService : IMeadowService
{
	private readonly MeadowLogic mLogic = new MeadowLogic();


	/// <summary>
	/// Get current season state.
	/// </summary>
	/// <returns>Current season state.</returns>				
	public Season getCurrentSeason()
	{
		return mLogic.GetSeasonState();
	}

	 public int GetUniqueId()
        {
            return mLogic.GetUniqueId();
        }

        public int GetGrassCount()
        {
            return mLogic.GetGrassCount();
        }

        public void AddToGrassCount(int grassGrowth)
        {
            mLogic.AddToGrassCount(grassGrowth);
        }

        public int GetBugCount()
        {
            return mLogic.GetBugCount();
        }

        public void AddToBugCount(int bugGrowth)
        {
            mLogic.AddToBugCount(bugGrowth);
        }

        public void RemoveFromBugCount(int bugDecline)
        {
            mLogic.RemoveFromBugCount(bugDecline);
        }

        public void RemoveGrass(int eatenGrass)
        {
            mLogic.RemoveGrass(eatenGrass);
        }
    }









