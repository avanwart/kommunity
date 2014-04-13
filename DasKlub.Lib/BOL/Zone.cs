using DasKlub.Lib.BaseTypes;

namespace DasKlub.Lib.BOL
{
    public class Zone : BaseIUserLogCRUD
    {
        private string _name = string.Empty;
        public int ZoneID { get; set; }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}