using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTB.Eshop.Application.ViewModels;

namespace UTB.Eshop.Application.Abstraction
{
    public interface IHomeService
    {
        CarouselZvireViewModel GetIndexViewModel();
    }
}
