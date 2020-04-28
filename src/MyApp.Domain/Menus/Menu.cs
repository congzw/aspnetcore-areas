namespace MyApp.Domain.Menus
{
    public class Menu
    {
        public string Key { get; set; }
        public string ParentKey { get; set; }
        public string FromArea { get; set; }
        public string Text { get; set; }
        public string Href { get; set; }
        public string CssClass { get; set; }
        public float Sort { get; set; }

        ////todo more
        //public string Area { get; set; }
        //public string Controller { get; set; }
        //public string Action { get; set; }
    }
}
