namespace JominiParse
{
    public interface IProgressFeedback
    {
        void StartNewJob(string text, int maximum);
        void AddProgress(int done);
    }
}