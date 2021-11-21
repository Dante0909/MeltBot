using PassionLib.Models;

namespace BlazorOfLimbo.Client.Service
{
    public interface IRunService
    {
        public Task<List<Run>> GetRuns();
    }
}
