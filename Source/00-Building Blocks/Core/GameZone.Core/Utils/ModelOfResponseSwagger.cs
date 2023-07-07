namespace GameZone.Core.Utils
{
    public class ModelOfResponseSwagger<T>
    {
        public bool success { get; set; } 
        public T data { get; set; }
    }
}
