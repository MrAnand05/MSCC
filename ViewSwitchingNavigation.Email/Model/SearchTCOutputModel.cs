using Microsoft.Practices.Prism.Mvvm;

namespace ViewSwitchingNavigation.Email.Model
{
    public class SearchTCOutputModel : BindableBase
    {
        public int Admino { get; set; }
        public string Name { get; set; }
        public string FName { get; set; }
        public string MName { get; set; }
        public int SNo { get; set; }
        public string Class { get; set; }
    }
}
