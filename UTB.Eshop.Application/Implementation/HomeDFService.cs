using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTB.Eshop.Application.Abstraction;
using UTB.Eshop.Application.ViewModels;
using UTB.Eshop.Infrastructure.Database;

namespace UTB.Eshop.Application.Implementation
{
    public class HomeDFService : IHomeService
    {
        public CarouselZvireViewModel GetIndexViewModel()
        {
            CarouselZvireViewModel viewModel = new CarouselZvireViewModel();
            viewModel.Zvirata = DatabaseFake.Zvirata;
            viewModel.Carousels = DatabaseFake.Carousels;
            return viewModel;
        }
    }
}
