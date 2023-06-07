using InitialProject.IRepository;
using InitialProject.Model;
using InitialProject.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitialProject.Repository
{
    public class GuideRepository : IGuideRepository
    {
        private UserRepository userRepository;

        private const string FilePathGuide = "../../../Resources/Data/guide.csv";

        private readonly Serializer<Guide> guideSerializer;

        private List<Guide> guides;

        public GuideRepository()
        {
            guideSerializer = new Serializer<Guide>();
        }

        public List<Guide> FindAll()
        {
            userRepository = new UserRepository();

            guides = guideSerializer.FromCSV(FilePathGuide);

            foreach (Guide temporaryGuide in guides.ToList())
            {
                temporaryGuide.User = userRepository.FindByUsername(temporaryGuide.User.Username);
            }

            return guides;

        }

        public Guide FindByUsername(string guideUsername)
        {
            return FindAll().ToList().Find(x => x.User.Username == guideUsername);
        }

        public void Save(List<Guide> guides)
        {
            guideSerializer.ToCSV(FilePathGuide, guides);
        }

    }
}
