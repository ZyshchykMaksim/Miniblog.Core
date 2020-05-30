namespace Miniblog.Core
{
    public class BlogSettings
    {
	    public string Name { get; set; } = "Miniblog";

        public int CommentsCloseAfterDays { get; set; } = 10;

        public PostListView ListView { get; set; } = PostListView.TitlesAndExcerpts;

        public string Owner { get; set; } = "The Owner";

        public int PostsPerPage { get; set; } = 4;
    }
}
