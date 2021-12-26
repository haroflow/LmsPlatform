namespace LmsPlatform.Dtos
{
	public class GetMyModuleDto
	{
		public string? Title { get; set; }
		public string? Slug { get; set; }
		public List<GetMyLessonDto>? Lessons { get; set; }
	}
}