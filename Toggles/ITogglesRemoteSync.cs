namespace Toggles
{
    public interface ITogglesRemoteSync
    {
        void StartConsuming();

        event ToggleUpdateEventHandler ToggleUpdate;
    }

    public delegate void ToggleUpdateEventHandler(Toggle toggle);
}