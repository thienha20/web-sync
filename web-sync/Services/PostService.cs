using web_sync.Repositories.ob;
using web_sync.Repositories.cb;

namespace web_sync.Services
{
    public class PostService
    {
        private readonly PostObRepository _postObRepository;
        private readonly PostCbRepository _postCbRepository;
        public PostService(PostObRepository postObRepository , PostCbRepository postCbRepository)
        {
            _postObRepository = postObRepository;
            _postCbRepository = postCbRepository;
        }
    }
}
