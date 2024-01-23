using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTB.Eshop.Domain.Entities;

namespace UTB.Eshop.Application.Abstraction
{
    public interface IZviratkoAppService
    {
        IList<Zviratko> Select();
        Task Create(Zviratko zviratko);
        bool Delete(int id);
    }
}
