using System.Collections.Generic;
using System.Linq;

namespace MyApp.Domain.Menus
{
    public class MenuContext
    {
        public MenuContext(IEnumerable<IMenuProvider> providers)
        {
            Menus = new List<Menu>();
            Init(providers);
        }

        public IList<Menu> Menus { get; set; }
        
        protected void Init(IEnumerable<IMenuProvider> providers)
        {
            if (providers != null)
            {
                var menuProviders = providers.OrderBy(x => x.Order).ToList();
                foreach (var menuProvider in menuProviders)
                {
                    menuProvider.ProcessMenu(this);
                }
            }
        }
    }
}