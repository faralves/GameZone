namespace GameZone.Core.ConfigApp
{
    public interface IConfigParameters
    {
        void SetGeneralConfig();
        void EnableConnectionLocal(bool enabled);
    }
}
