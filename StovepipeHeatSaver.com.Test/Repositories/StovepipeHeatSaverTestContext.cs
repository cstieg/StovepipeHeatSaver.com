using StovepipeHeatSaver.Models;

namespace StovepipeHeatSaver.com.Test.Repositories
{
    public class StovepipeHeatSaverTestContext : ApplicationDbContext
    {
        public StovepipeHeatSaverTestContext() : base("StovePipeHeatSaverTest") { }
    }
}
