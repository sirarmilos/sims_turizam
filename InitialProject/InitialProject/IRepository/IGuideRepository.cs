using InitialProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.IRepository
{
    public interface IGuideRepository
    {
        List<Guide> FindAll();

        Guide FindByUsername(string guideUsername);

        void Save(List<Guide> guides);
    }
}
