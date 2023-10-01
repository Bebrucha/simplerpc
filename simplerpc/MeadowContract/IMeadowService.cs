using System;

namespace Services
{

    public class BugDesc
    {
        public int bugId { get; set; }
        public int bugGrowth { get; set; }

        public int bugDecline { get; set; }

        public int bugEats { get; set; }
    }
    
    public class GrassDesc{
      public int grassId{get;set;}

      public int grassGrowth {get;set;}

    }
    public interface IMeadowService
    {
    
        int GetUniqueId();
        Season getCurrentSeason();
        int GetGrassCount();
        void AddToGrassCount(int grassGrowth);
        int GetBugCount(); 
        void AddToBugCount(int bugGrowth);
        void RemoveFromBugCount(int bugDecline);
        void RemoveGrass(int eatenGrass);
    }

    public enum Season
    {
        Spring,
        Summer,
        Fall,
        Winter
    }
}
