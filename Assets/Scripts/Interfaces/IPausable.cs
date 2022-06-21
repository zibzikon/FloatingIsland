namespace Interfaces
{
    public interface IPausable
    {
        public bool IsPaused { get; }
        void Pause();
        void UnPause();
    }
}