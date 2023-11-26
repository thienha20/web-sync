using sync_data.Repositories.ob;
using sync_data.Repositories.cb;

namespace sync_data.Services
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
