namespace ControlDoc.Models.Models.ComponentViews.Menu.Request
{
    public partial class SidebarItems
    {
        public string ItemName { get; set; }
        public string IconPath { get; set; }
        public bool Active { get; set; }

        public bool ImageChange { get; set; }

        public List<SidebarSubItems> SubItems { get; set; }
    }
}
