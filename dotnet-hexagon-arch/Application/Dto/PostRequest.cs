namespace MatrigraniCiro.HexagonArch.BestBlogs.Application.Dto
{
    public record PostRequest
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
}