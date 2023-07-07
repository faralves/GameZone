using System.ComponentModel;

namespace GameZone.Core.Utils
{
    public class ModelOfErrorsSwagger
    {
        public bool success { get; set; }

        public IEnumerable<BindingList<string>> errors { get; set; }
    }
}
