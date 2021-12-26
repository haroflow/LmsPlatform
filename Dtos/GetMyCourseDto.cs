namespace LmsPlatform.Dtos
{
	public class GetMyCourseDto
	{
		public string? Slug { get; set; }
		public string? Name { get; set; }
		public string? Description { get; set; }
		public string? BannerImage { get; set; }
		public List<GetMyModuleDto>? Modules { get; set; }
	}
}