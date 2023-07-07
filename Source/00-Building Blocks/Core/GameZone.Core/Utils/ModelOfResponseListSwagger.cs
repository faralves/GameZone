namespace GameZone.Core.Utils
{
    public class ModelOfResponseListSwagger<T>
    {
        public bool success { get; set; } 
        public IEnumerable<T> data { get; set; }
    }
}
