using MyApp.Domain.Menus;

namespace MyApp.Web.Areas.Demo.Libs.Domain
{
    public class DemoMenuProvider : IMenuProvider
    {
        public int Order { get; set; } = 0;

        public void ProcessMenu(MenuContext menuContext)
        {
            menuContext.Menus.Add(new Menu() { FromArea = "Demo", Href = "/Demo/Home/Index", Key = "/Demo/Home/Index", ParentKey = "/", Text = "DemoHomeIndex" });
            menuContext.Menus.Add(new Menu() { FromArea = "Demo", Href = "/Demo/Home/DiTest", Key = "/Demo/Home/DiTest", ParentKey = "/", Text = "DemoHomeDiTest" });
        }
    }
}
