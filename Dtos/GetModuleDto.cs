namespace LmsPlatform.Dtos
{
	public class GetModuleDto
	{
		public string? Title { get; set; }
		public string? Slug { get; set; }
		public List<GetLessonDto>? Lessons { get; set; }
	}
}