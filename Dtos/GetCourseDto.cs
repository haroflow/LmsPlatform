namespace LmsPlatform.Dtos
{
	public class GetCourseDto
	{
		public string? Slug { get; set; }
		public string? Name { get; set; }
		public string? Description { get; set; }
		public string? BannerImage { get; set; }
		public List<GetModuleDto>? Modules { get; set; }
		public bool UserEnrolled { get; set; } = false;
	}
}