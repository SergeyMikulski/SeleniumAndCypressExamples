using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.FunctionalTests.PageObjects
{
    public abstract class PageObjectBase
    {
        protected readonly IAutomateBrowser Browser;

        protected PageObjectBase(IAutomateBrowser browser)
        {
            Browser = browser;
        }
    }
}
