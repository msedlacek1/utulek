using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UTB.Eshop.Domain.Entities;

namespace UTB.Eshop.Application.Abstraction
{
    public interface IZvireAppService
    {
        IList<Zvire> Select();
        Task Create(Zvire zvire);
        bool Delete(int id);
    }
}
