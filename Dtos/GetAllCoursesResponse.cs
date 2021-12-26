namespace LmsPlatform.Dtos
{
	public class GetAllCoursesResponse
	{
		public string? Slug { get; set; }
		public string? Name { get; set; }
		public string? Description { get; set; }
		public string? MiniatureImage { get; set; }
		public bool UserEnrolled { get; set; }
	}
}